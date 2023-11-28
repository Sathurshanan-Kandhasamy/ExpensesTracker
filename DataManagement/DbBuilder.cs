using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DataManagement.Models;

namespace DataManagement
{
    /// <summary>
    /// Database building class to programatically build the application database, tables and testing data.
    /// Inherits form the DataAdapter class to allow usage of some of its save methods.
    /// 
    /// This class is triggered from the App.xaml.cs constructor on startup.
    /// </summary>
    public class DbBuilder: DataAdapter
    {
        /// <summary>
        /// Sends a request to SQL Server to check if a databse exists matching name provided in the connection string. 
        /// If it does not exists the query then asks the
        /// server to create a new databse with the provided connection string name.
        /// </summary>
        public void CreateDatabase()
        {
            //Our connection object to link to the database
            SqlConnection connection = Helper.CreateSQLServerConnection("Default");
            try
            {
                //Custom connection string to only connect to the server layer of your SQL Database
                string connectionString = $"Data Source={connection.DataSource}; Integrated Security = True";
                //Query to build new Database if it does not already exist.
                string query = $"IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name ='{connection.Database}') " +
                $" CREATE DATABASE {connection.Database}";
                using (connection = new SqlConnection(connectionString))
                {
                    //A command object which will send our request to the Database <= Normally done for us by Dapper
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        //Checks if the connection is currently open, if not, it opens the connection.<= Normally done for us by Dapper
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        //Executes an SQL Request that does not expect a response(Query) to be returned.
                        command.ExecuteNonQuery();
                        //Closes the connection to the database manually.<= Normally done for us by Dapper
                        connection.Close();
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        /// <summary>
        /// Runs a query against the database to get a count of how many base tables exist in the database.
        /// </summary>
        /// <returns>A confirmation of whther there are tables (TRUE) or not (FALSE)</returns>
       public bool DoTablesExist()
       {
            //Our using statemtnqwhich builds our connection and disposes of it once finished.
            using (var connection = Helper.CreateSQLServerConnection("Default"))
            {
                //Quey to request the count of how many base tables are in the database structure. Base tables refers to user
                //built tables and ignores inbuild tables such as index tables and reference/settings tables.
                string query = $"SELECT COUNT(*) FROM {connection.Database}.INFORMATION_SCHEMA.TABLES " +
                $"WHERE TABLE_TYPE = 'BASE TABLE'";
                //Sends the query to the databse and stores the returned table count.
                int count = connection.QuerySingle<int>(query);
                //If the count is above 0 return true, otherwise return false to indicate whether the databse has tabes or not.
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Method to send a request to the databse to create a new databse table. This method requires the table 
        /// name and column/attributes details to be pre-populated and
        /// passed to the method for it to work.
        /// </summary>
        /// <param name="name">The name to be given to the table when created.</param>
        /// <param name="structure">A string oulining all the table column/attributes and their names, 
        ///                         types and any other special rules for each of them such as PK
        /// identification, nullability rules and foreighn key connections.</param>
        private void CreateTable(string name, string structure)
        {
            try
            {
                //Partial query to build table in database. Parameters passe to method will be inserted to complete the query string.
                string query = $"CREATE TABLE {name} ({structure})";
                //Our using statemtnqwhich builds our connection and disposes of it once finished.
                using (var connection = Helper.CreateSQLServerConnection("Default"))
                {
                    //Passes the query to the databse to be perfomed.
                    connection.Execute(query);
                }
            }
            catch (Exception e)
            {
                //Log error on failure
            }
        }

        /// <summary>
        /// Triggers the database table creation and database seeding methods in the correct sequence to 
        /// ensure they are all generated properly and to avoid foreign key errors which can be caused by builkding or
        /// seeding the tables with foreign keys before the referenced tables or data have been built.
        /// </summary>
        public void BuildDatabaseTables()
        {
            BuildUsersTable();
            BuildCategoriesTable();
            BuildExpensesTable();

            SeedUsersTable();
            SeedCategoriesTable();
            SeedExpensesTable();
        }

        /// <summary>
        /// Defines the structure of the User table before passing it to the CreateTable method to be built 
        /// in the database.
        /// </summary>
        private void BuildUsersTable()
        {
            //Defines the column names and attributes for the table
            string structure = "Id int PRIMARY KEY IDENTITY(1,1), " +
                               "Name VARCHAR(100) NOT NULL, " +
                               "Role VARCHAR(50) NOT NULL";
            
            CreateTable("Users", structure);
        }

        /// <summary>
        /// Defines the structure of the Categories table before passing it to the CreateTable method to be built 
        /// in the database.
        /// </summary>
        private void BuildCategoriesTable()
        {
            //Defines the column names and attributes for the table
            string structure = "Id int PRIMARY KEY IDENTITY(1,1), " +
                               "Name VARCHAR(50) NOT NULL";

            CreateTable("Categories", structure);
        }

        /// <summary>
        /// Defines the structure of the Expenses table before passing it to the CreateTable method to be built 
        /// in the database.
        /// </summary>
        private void BuildExpensesTable()
        {
            //Defines the column names, attributes and foreign keys for the table
            string structure = "Id int PRIMARY KEY IDENTITY(1,1), " +
                               "Date DATETIME NOT NULL, " +
                               "CategoryId int NOT NULL, " +
                               "UserId int NOT NULL, " +
                               "Price MONEY NOT NULL " +
                               "FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ," +
                               "FOREIGN KEY (UserId) REFERENCES Users(Id)";

            CreateTable("Expenses", structure);
        }

        private void SeedUsersTable()
        {
            //Create list to hold entriews that are being added.
            List<User> users = new List<User>();
            //Add new entries to list and provide them with sample data.
            users.Add(new User 
            {   Id = 1, 
                Name = "Troy Vaughn",
                Role = "Admin"
            });
            users.Add(new User
            {
                Id = 2,
                Name = "Jane Lane",
                Role = "Accounts"
            });
            users.Add(new User
            {
                Id = 3,
                Name = "Gary Barlow",
                Role = "Sales"
            });
            //Iterate through the list and pass each entry to the addd method to send it to the database.
            foreach (var user in users)
            {
                AddNewUser(user);
            }
        }

        private void SeedCategoriesTable()
        {
            //Create list to hold entriews that are being added.
            List<Category> categories = new List<Category>();
            //Add new entries to list and provide them with sample data.
            categories.Add(new Category
            {
                Id = 1,
                Name= "Purchases",
            });
            categories.Add(new Category
            {
                Id = 2,
                Name = "Payroll",
            });
            categories.Add(new Category
            {
                Id = 3,
                Name = "Utilities",
            });
            //Iterate through the list and pass each entry to the addd method to send it to the database.
            foreach (var category in categories)
            {
                AddNewCategory(category);
            }
        }

        private void SeedExpensesTable()
        {
            //Create list to hold entriews that are being added.
            List<Expense> expenses = new List<Expense>();
            //Add new entries to list and provide them with sample data.
            expenses.Add(new Expense 
            {
                Id = 1,
                Price= 10.99M,
                UserId= 1,
                CategoryId= 1,
                Date= DateTime.Now.AddDays(-12)
            });
            expenses.Add(new Expense
            {
                Id = 1,
                Price = 200.99M,
                UserId = 3,
                CategoryId = 2,
                Date = DateTime.Now.AddDays(-52)
            });
            expenses.Add(new Expense
            {
                Id = 1,
                Price = 599.95M,
                UserId = 2,
                CategoryId = 3,
                Date = DateTime.Now.AddDays(-112)
            });
            //Iterate through the list and pass each entry to the addd method to send it to the database.
            foreach (var expense in expenses)
            {
                SaveNewExpense(expense);
            }
        }

    }
}
