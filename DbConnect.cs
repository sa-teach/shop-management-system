using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Shop_Management_System
{
    internal class DbConnect
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cd = new SqlCommand();
        private string con;

        public string Connection() 
        {
            // Используем относительный путь к базе данных в папке bin\Debug
            string dbPath = System.IO.Path.Combine(Application.StartupPath, "dbHikeShop.mdf");
            con = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;Connect Timeout=30";
            return con;
        }
    }
}
