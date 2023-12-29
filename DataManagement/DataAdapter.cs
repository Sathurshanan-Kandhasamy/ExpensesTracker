using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;
using DataManagement.Models;

namespace DataManagement
{
    public class DataAdapter
    {
        #region Users

        public List<User> GetAllUsers()
        {
            //The SQL Query to be sent to the database with the request.
            string query = "SELECT * FROM Users";

            // A using statement to manage the connection resource(object) which will dispose
            // of the resource once it is finished.
            using (var connection = Helper.CreateSQLServerConnection("Default"))
            {
                // Request to be sent to the database.
                return connection.Query<User>(query).ToList();
            }
        }

        public User GetUserById(int id)
        {
            //The SQL Query to be sent to the database with the request.
            string query = $"SELECT * FROM Users WHERE Id = {id}";

            // A using statement to manage the connection resource(object) which will dispose
            // of the resource once it is finished.
            using (var connection = Helper.CreateSQLServerConnection("Default"))
            {
                // Request to be sent to the database.
                return connection.QuerySingle<User>(query);
            }
        }

        public void AddNewUser(User userEntry)
        {
            //The SQL Query to be sent to the database with the request.
            string query = "INSERT INTO Users (Name,Role) " +
                           "VALUES (@Name,@Role)";

            // A using statement to manage the connection resource(object) which will dispose
            // of the resource once it is finished.
            using (var connection = Helper.CreateSQLServerConnection("Default"))
            {
                // Request to be sent to the database.
                connection.Execute(query, userEntry);
            }
        }

        public void UpdateUser(User userEntry)
        {
            //The SQL Query to be sent to the database with the request.
            string query = "UPDATE Users " +
                           "SET Name = @Name, Role = @Role " +
                           "WHERE Id = @Id";

            // A using statement to manage the connection resource(object) which will dispose
            // of the resource once it is finished.
            using (var connection = Helper.CreateSQLServerConnection("Default"))
            {
                // Request to be sent to the database.
                connection.Execute(query, userEntry);
            }
        }

        public void DeleteUser(int id)
        {
            //The SQL Query to be sent to the database with the request.
            string query = "DELETE FROM Users " +
                          $"WHERE Id = {id}";

            // A using statement to manage the connection resource(object) which will dispose
            // of the resource once it is finished.
            using (var connection = Helper.CreateSQLServerConnection("Default"))
            {
                // Request to be sent to the database.
                connection.Execute(query);
            }
        }

        #endregion

        #region Categories

        public List<Category> GetAllCategories()
        {
            //The SQL Query to be sent to the database with the request.
            string query = "SELECT * FROM Categories";

            // A using statement to manage the connection resource(object) which will dispose
            // of the resource once it is finished.
            using (var connection = Helper.CreateSQLServerConnection("Default"))
            {
                // Request to be sent to the database.
                return connection.Query<Category>(query).ToList();
            }
        }

        public void AddNewCategory(Category categoryEntry)
        {
            //The SQL Query to be sent to the database with the request.
            string query = "INSERT INTO Categories (Name) " +
                           "VALUES (@Name)";

            // A using statement to manage the connection resource(object) which will dispose
            // of the resource once it is finished.
            using (var connection = Helper.CreateSQLServerConnection("Default"))
            {
                // Request to be sent to the database.
                connection.Execute(query, categoryEntry);
            }
        }

        #endregion

        #region Expenses

        public List<ExpenseView> GetAllExpenses()
        {
            //The SQL Query to be sent to the database with the request.
            string query = "SELECT Expenses.Id, Expenses.Date, Expenses.Price, " +
                           "Users.Name AS UserName, Categories.Name AS CategoryName " +
                           "FROM Categories " +
                           "INNER JOIN " +
                           "Expenses ON Categories.Id = Expenses.CategoryId " +
                           "INNER JOIN " +
                           "Users ON Expenses.UserId = Users.Id " +
                           "ORDER BY Expenses.Date DESC";

            // A using statement to manage the connection resource(object) which will dispose
            // of the resource once it is finished.
            using (var connection = Helper.CreateSQLServerConnection("Default"))
            {
                // Request to be sent to the database.
                return connection.Query<ExpenseView>(query).ToList();
            }
        }

        public Expense GetExpenseById(int id)
        {
            //The SQL Query to be sent to the database with the request.
            string query = $"SELECT * FROM Expenses WHERE Id = {id}";

            // A using statement to manage the connection resource(object) which will dispose
            // of the resource once it is finished.
            using (var connection = Helper.CreateSQLServerConnection("Default"))
            {
                // Request to be sent to the database.
                return connection.QuerySingle<Expense>(query);
            }
        }

        public void SaveNewExpense(Expense currentExpense)
        {
            //The SQL Query to be sent to the database with the request.
            string query = "INSERT INTO Expenses (Price,Date,UserId,CategoryId) " +
                           "VALUES (@Price,@Date,@UserId,@CategoryId)";

            // A using statement to manage the connection resource(object) which will dispose
            // of the resource once it is finished.
            using (var connection = Helper.CreateSQLServerConnection("Default"))
            {
                // Request to be sent to the database.
                connection.Execute(query, currentExpense);
            }
        }

        public void UpdateExpense(Expense expenseEntry)
        {
            //The SQL Query to be sent to the database with the request.
            string query = "UPDATE Expenses " +
                           "SET Price = @Price, Date = @Date, UserId = @UserId, CategoryId = @CategoryId " +
                           "WHERE Id = @Id";

            // A using statement to manage the connection resource(object) which will dispose
            // of the resource once it is finished.
            using (var connection = Helper.CreateSQLServerConnection("Default"))
            {
                // Request to be sent to the database.
                connection.Execute(query, expenseEntry);
            }
        }

        public void DeleteExpense(int id)
        {
            //The SQL Query to be sent to the database with the request.
            string query = "DELETE FROM Expenses " +
                          $"WHERE Id = {id}";

            // A using statement to manage the connection resource(object) which will dispose
            // of the resource once it is finished.
            using (var connection = Helper.CreateSQLServerConnection("Default"))
            {
                // Request to be sent to the database.
                connection.Execute(query);
            }
        }

        #endregion
    }
}
