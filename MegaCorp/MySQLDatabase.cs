using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace MegaCorp
{
    public class MySQLDatabase : Database
    {
        IDbConnection dbConnection;
        DBConfig _config;
        public MySQLDatabase(DBConfig config)
        {
            _config = config;
            Connect();
        }

        public void Connect()
        {
            try
            {
                dbConnection = new MySqlConnection(_config.ConnectionString);
                dbConnection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Create<T>(string table, T obj)
        {
            IDbCommand cmd = dbConnection.CreateCommand();

            cmd.CommandText = GenerateCommand("INSERT INTO", table, obj);
            cmd.ExecuteNonQuery();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Read()
        {
            throw new NotImplementedException();
        }

        public void Update<T>(string table, T obj)
        {
            IDbCommand cmd = dbConnection.CreateCommand();

            cmd.CommandText = GenerateCommand("UPDATE", table, obj);
            cmd.ExecuteNonQuery();
        }

        public void Disconnect()
        {
            dbConnection.Close();
        }

        ~MySQLDatabase()
        {
            Disconnect();
        }

        private string GenerateCommand<T>(string action, string table, T obj)
        {
            string command = $"{action} {table} SET ";
            int id = 0;
            // Objekttyp bestimmen und alle Eigenschaften einlesen.
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            // Anzahl der Objekt Eigenschaften und ein Zähler um zu bestimmen,
            // wann die letzte Eigenschaft gelesen wurde, damit an dem Command String
            // entweder ein Komma oder ein Lehrzeichen angefügt werden kann.
            int length = properties.Length;
            int i = 1;

            // Einzelne Eigenschaften auslesen.
            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;

                // Da wir hier wissen welche Eigenschaften der Typ hat,
                // können wir diese gezielt aus dem als Parameter übergebenden Objekt
                // auslesen.
                object propertyValue = property.GetValue(obj, null);
                string propertyType = property.PropertyType.Name;

                // Die Eigenschaft "Id" wird speziell behandelt.
                // Wenn die Id == 0 ist, wird die Eigenschaft als NULL
                // in die Datenbank geschrieben um die auto_increment funktion
                // von MySQL auszulösen.
                // Ansonsten wird ein neuer Eintrag mit festgelegter Id erstellt.
                if (propertyName == "Id")
                {
                    command += (int)propertyValue == 0 ? $"{propertyName} = NULL, " : $"{propertyName} = {propertyValue}, ";
                    id = (int)propertyValue;
                    i++;
                    continue;
                }

                // Hier sollte die Prüfung folgen, ob die Eigenschaft einen Wert hat.
                // Wenn eine Klasse mehrere Konstruktoren hat, führt eine Undefinierte Eigenschaft zu fehlern.
                // FUNKTIONIERT ABER NICHT :D
                // TODO: propertyValue auf Wert prüfen...
                if (typeof(T).GetProperty(property.Name) != null)
                {
                    // Unterscheidung der Eigenschaftstypen. Ein String wird mit,
                    // und ein Integer ohne Hochkomma dem Command String hinzugefügt.
                    switch (propertyType)
                    {
                        case "Int32":
                            command += $"{propertyName} = {propertyValue}";
                            break;
                        case "String":
                            command += $"{propertyName} = '{propertyValue}'";
                            break;
                        case "DateTime":
                            DateTime date = Convert.ToDateTime(propertyValue);
                            string MySQLDate = date.ToString("yyyy-MM-dd");
                            command += $"{propertyName} = '{MySQLDate}'";
                            break;
                        case "Boolean":
                            command += $"{propertyName} = {propertyValue}";
                            break;
                    }

                    // Sind alle Eigenschaften ausgelesen?
                    command += (length == i) ? " " : ", ";
                    i++;
                }
            }

            command += (action == "UPDATE") ? $"WHERE Id={id}" : "";
            command += ";";

            return command;
        }

        private IDataReader sooperDooperMysqlFuncyWunky(string q)
        {
            IDbCommand cmd = dbConnection.CreateCommand();
            cmd.CommandText = q;
            IDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        // Nur für im System bekannte Datenbanken wie SQL, ORACLE etc.
        //static DataTable GetProviderFactoryClasses()
        //{
        //    // Retrieve the installed providers and factories.
        //    DataTable table = DbProviderFactories.GetFactoryClasses();

        //    // Display each row and column value.
        //    foreach (DataRow row in table.Rows)
        //    {
        //        foreach (DataColumn column in table.Columns)
        //        {
        //            MessageBox.Show(row[column].ToString());
        //        }
        //    }
        //    return table;
        //}
    }
}
