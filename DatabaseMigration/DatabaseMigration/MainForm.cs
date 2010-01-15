using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace DatabaseMigration
{
    public partial class MainForm : Form
    {
        public MainForm()
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
                // See if the database is there
                var theDatabases = dc.ExecuteQuery<Linq.Databases>(@"SELECT name from sysdatabases");
                bool foundDatabase = false;
                foreach (Linq.Databases db in theDatabases)
                {
                    if (db.name == "CRMOnline")
                    {
                        foundDatabase = true;
                    }
                }

                if (!foundDatabase)
                {
                    // Need to create the database
                    dc.ExecuteCommand(@"CREATE DATABASE CRMOnline");
                }

                // OK, at this point we can assume the database is there.
                // Look for the MigrationLevel table
                var theTables = dc.ExecuteQuery<Linq.Tables>(@"SELECT name, xtype from CRMOnline.dbo.sysobjects where xtype='U'");
                bool foundTable = false;
                foreach (Linq.Tables t in theTables)
                {
                    if (t.name == "MigrationLevel")
                    {
                        foundTable = true;
                    }
                }

                if (!foundTable)
                {
                    // Need to create the table
                    dc.ExecuteCommand(@"CREATE TABLE CRMOnline.dbo.MigrationLevel (MigrationNumber int not null, MigrationDate datetime not null)");
                }

                // OK, now it is safe to assume that the key table and the database are present.
                // Run through the migrations.
                MigrateData(dc);
                MessageBox.Show("Migration complete.");
                this.Close();
            }
            catch (Exception theException)
            {
                MessageBox.Show(string.Format("Error:  {0}", theException.Message));
                return;
            }
        }

        void MigrateData(DataClasses1DataContext dc)
        {
            // Spin through the migrations and run them if they don't already exist.
            DirectoryInfo di = new DirectoryInfo("..\\..\\Migrations");
            FileInfo[] rgFiles = di.GetFiles("*.txt");
            foreach (FileInfo fi in rgFiles)
            {
                string migrationLevel = fi.Name.Substring(0, fi.Name.Length - 4);
                var results = dc.ExecuteQuery<Linq.MigrationLevel>(string.Format("select * from CRMOnline.dbo.MigrationLevel where MigrationNumber={0}", migrationLevel));
                bool hasResults = false;
                foreach (Linq.MigrationLevel level in results)
                {
                    hasResults = true;
                }

                if (!hasResults)
                {
                    // Read the contents of the file and execute it as a SQL command
                    StreamReader s = File.OpenText(string.Format("..\\..\\Migrations\\{0}.txt", migrationLevel));
                    string theString = null;
                    theString = s.ReadToEnd();

                    try
                    {
                        dc.ExecuteCommand(theString);
                        dc.ExecuteCommand(string.Format("insert into CRMOnline.dbo.MigrationLevel(MigrationNumber, MigrationDate) values({0}, getdate())", migrationLevel));
                    }
                    catch(Exception theException)
                    {
                        MessageBox.Show(string.Format("Error on migration#{0}: {1}", migrationLevel, theException.Message));
                    }

                    s.Close();
                }
            }
        }
    }
}
