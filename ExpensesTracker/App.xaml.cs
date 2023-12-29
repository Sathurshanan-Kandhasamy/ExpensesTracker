using DataManagement;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows;

namespace ExpensesTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // Create database building class.
            DbBuilder builder = new DbBuilder();
            // If database doesn't already exist, build new one.
            builder.CreateDatabase();
            // Check if the database contains tables. If not, run the table building code. 
            if (builder.DoTablesExist() == false)
            {
                // Runs the code to build and seed the database tables.
                builder.BuildDatabaseTables();  
            }
        }
    }
}
