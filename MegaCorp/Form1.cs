using System;
using System.Windows.Forms;

namespace MegaCorp
{
    public partial class Form1 : Form
    {
        Database db = new MySQLDatabase(new DBConfig());

        public Form1()
        {
            InitializeComponent();
            db.Create("employees", new Employee(0, "Hans", "Wurst", new DateTime(1890, 10, 30)));
        }
    }
}
