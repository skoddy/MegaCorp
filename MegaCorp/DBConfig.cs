using System.Configuration;

namespace MegaCorp
{
    public class DBConfig
    {
        public string ConnectionString { get; }

        public DBConfig()
        {
            ConnectionString = GetConnectionString("MySQL");
        }

        static string GetConnectionString(string name)
        {
            string returnValue = null;

            ConnectionStringSettingsCollection settings =
                ConfigurationManager.ConnectionStrings;

            if (settings != null)
            {
                foreach (ConnectionStringSettings cs in settings)
                {
                    if (cs.Name == name)
                        returnValue = cs.ConnectionString;
                    break;
                }
            }
            return returnValue;
        }
    }
}
