using DataManagement;
using DataManagement.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExpensesTracker
{
    /// <summary>
    /// Interaction logic for ReportsPanel.xaml
    /// </summary>
    public partial class ReportsPanel : UserControl
    {
        //Class for managing all our SQL requests form the database for our data models.
        DataAdapter data = new DataAdapter();

        //Lists for managing my User reports.
        List<User> fullUserList;
        List<User> displayUserList;
        //Lists for managing thr expenses reports.
        List<ExpenseView> fullExpenseList;
        List<ExpenseView> displayExpenseList;


        public ReportsPanel()
        {
            InitializeComponent();

            //Create an array with the report options for the combobox.
            string[] reportOptions = {"User By Name", "Expense by Price", "Expense by Category"};
            //Assign the array of options to the combobox.
            cboReports.ItemsSource = reportOptions;
        }

        //Triggers when the combobox slecetion is changed.
        private void cboReports_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //If the selected value of the combo box is the blank option, retuyrn out of ther method.
            if (cboReports.SelectedIndex < 0)
            {
                return;
            }
            //Swith on the current selected index of the combobox and run the associated code.
            switch (cboReports.SelectedIndex)
            {
                case 0:
                    //Retrieve all the users from the database and order them by name using a LINQ atatement.
                    //NOTE: LINQ is a code based query language used for filetring and sorting collecitons.
                    fullUserList = data.GetAllUsers().OrderBy(user => user.Name).ToList();
                    //Assign the returned list to the display list which will be used in the datagrid and for exporting.
                    displayUserList = fullUserList;
                    //Pass the list to the method used for setting the datasource for the datagrid
                    UpdateDataGrid(displayUserList);
                    break;

                case 1:
                    //Retrieve all the expenses from the database and order them by Price using a LINQ atatement.
                    fullExpenseList = data.GetAllExpenses().OrderByDescending(expense => expense.Price).ToList();
                    //Assign the returned list to the display list which will be used in the datagrid and for exporting.
                    displayExpenseList = fullExpenseList;
                    //Pass the list to the method used for setting the datasource for the datagrid
                    UpdateDataGrid(displayExpenseList);
                    break;

                case 2:
                    //Retrieve all the expenses from the database and order them by Category Name using a LINQ atatement.
                    fullExpenseList = data.GetAllExpenses().OrderBy(expense => expense.CategoryName).ToList();
                    //Assign the returned list to the display list which will be used in the datagrid and for exporting.
                    displayExpenseList = fullExpenseList;
                    //Pass the list to the method used for setting the datasource for the datagrid
                    UpdateDataGrid(displayExpenseList);
                    break;
            }
        }

        /// <summary>
        /// Takes a list and then sets it as the itemsource for the datagrid. This method uses Generics (as indicated by the T placeholder)
        /// which takes the provided list and sets T based upon the provided type. 
        /// </summary>
        /// <typeparam name="T">The Type to be used in this method</typeparam>
        /// <param name="displayList">The list to be assigned to the datagrid</param>
        private void UpdateDataGrid<T>(List<T> displayList)
        {
            //Set the provided list as the source of data for the datagrid.
            dgvReports.ItemsSource = displayList;
            //Refresh the datagrid display.
            dgvReports.Items.Refresh();
        }

        //Triggers whenever the text in the search textbox changes
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //If the current combobox option is the blank entry, return and do nothing. Otherwise, run the correct filter method for the 
            // selected report type.
            if (cboReports.SelectedIndex < 0)
            {
                return;
            }
            else if (cboReports.SelectedIndex == 0)
            {
                FilterUserReport();
            }
            else 
            {
                FilterExpenseReport();
            }
        }

        private void FilterUserReport()
        {
            displayUserList = fullUserList.Where(user => user.Name.Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
            UpdateDataGrid(displayUserList);
        }

        private void FilterExpenseReport()
        {
            displayExpenseList = fullExpenseList.Where(expense => expense.UserName
                                                        .Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList(); 
            UpdateDataGrid(displayExpenseList);
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {

            if (cboReports.SelectedIndex < 0)
            {
                return;
            }
            else if (cboReports.SelectedIndex == 0)
            {
                ExportToCSV(displayUserList);
            }
            else
            {
                ExportToCSV(displayExpenseList);
            }
        }

        private void ExportToCSV<T>(List<T> exportList)
        {
            //Creates the file dialog that lets us select a file location for our data.
            SaveFileDialog dialog = new SaveFileDialog();
            //Sets the filters for the allowewd file types of the dialog.
            dialog.Filter = "Comma-Delimitted-Values|*.csv|" +
                            "Plain Text File|*.txt";
            //Sets the default file location for the dialog to be the MyDocuments folder.
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //Opens the file dialog from within the if statement and if it is used sucessfully (valid file name and save button pressed)
            //the if statement logic will run.
            if (dialog.ShowDialog() == true)
            {
                using (StreamWriter writer = new StreamWriter(dialog.FileName))
                {
                    foreach (var item in exportList)
                    {
                        writer.WriteLine(item.ToString());
                    }
                }
            }
        }

    }
}
