using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
