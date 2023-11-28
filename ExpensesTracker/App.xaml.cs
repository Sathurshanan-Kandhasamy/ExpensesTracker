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
            //Create our Database building class.
            DbBuilder builder = new DbBuilder();
            //Tell it ti build the database. This will only do so if it doesn't already exist.
            builder.CreateDatabase();
            //Check if the database contains tables. If not, run the table building code. 
            if (builder.DoTablesExist() == false)
            {
                //Runs the code to build and seed the database tables.
                builder.BuildDatabaseTables();  
            }
        }
    }
}
