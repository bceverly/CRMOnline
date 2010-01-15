using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DatabaseMigration
{
    class Tables
    {
        string TABLE_NAME { get; set; }
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnMigrate_Click(object sender, EventArgs e)
        {
            string connectionString = string.Format("Data Source={0};Initial Catalog=master;User ID={1};Password={2}",
                tbDatabaseServer.Text, tbUserId.Text, tbPassword.Text);

            SqlConnection connection = new SqlConnection(connectionString);
            DataClasses1DataContext dc = new DataClasses1DataContext(connection);

            try
            {
                var results = dc.ExecuteQuery<Tables>(@"SELECT TABLE_NAME from INFORMATION_SCHEMA.TABLES");
            }
            catch (Exception theException)
            {
                MessageBox.Show(string.Format("Error:  {0}", theException.Message));
                return;
            }
        }
    }
}
