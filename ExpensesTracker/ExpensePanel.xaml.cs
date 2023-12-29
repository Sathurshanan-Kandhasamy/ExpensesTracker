using System;
using System.Collections.Generic;
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
using DataManagement;
using DataManagement.Models;

namespace ExpensesTracker
{
    /// <summary>
    /// Interaction logic for ExpensePanel.xaml
    /// </summary>
    public partial class ExpensePanel : UserControl
    {
        // The class object used to communicate with the Database. 
        DataAdapter data = new DataAdapter();

        // Lists used to populate the combo boxes.
        List<User> userList = new List<User>();
        List<Category> categoryList = new List<Category>();
        // Lists used to populate the data grid.
        List<ExpenseView> expenseList = new List<ExpenseView>();

        // Acts as a flag to indicate the correct save mode (NEW or UPDATE)
        bool isNewEntry = true;

        public ExpensePanel()
        {
            InitializeComponent();
            SetupComboBoxes();
            UpdateDataGrid();
            pkrDate.IsTodayHighlighted= false;
        }

        private void UpdateDataGrid()
        {
            expenseList = data.GetAllExpenses();
            dgvExpenses.ItemsSource = expenseList;
            dgvExpenses.Items.Refresh();
        }

        private void SetupComboBoxes()
        {
            // Gets the Users from the database.
            userList = data.GetAllUsers();
            // Sets the list as the source of the combo box. 
            cboUsers.ItemsSource = userList;
            // Sets the Name property of each list item as what displays in the combo box.
            cboUsers.DisplayMemberPath = "Name";
            // Sets the value that is returned when a combo box option is selected.
            cboUsers.SelectedValuePath = "Id";

            // Repeats the same process for the categories combo box.
            categoryList = data.GetAllCategories();
            cboCategory.ItemsSource = categoryList;
            cboCategory.DisplayMemberPath = "Name";
            cboCategory.SelectedValuePath = "Id";
        }

        private void ClearDataEntryFields()
        {
            // Set the form text fields to blank.
            txtId.Text = string.Empty;
            txtPrice.Text = string.Empty;
            // Set the form combo boxes back to no entry selected.
            cboCategory.SelectedIndex = -1;
            cboUsers.SelectedIndex = -1;
            // Set the calendar data back to todays current date.
            pkrDate.SelectedDate = DateTime.Today;

            isNewEntry = true;
        }

        /// <summary>
        /// Checks all the data fields of the form to see if they hold valid data for saving.
        /// If any field is invalid the method returns false.
        /// </summary>
        /// <returns>Whether the data fields contain valid data.</returns>
        private bool IsFormFilledCorrectly()
        {
            // Check if the price field holds a valid decimal number.
            if (decimal.TryParse(txtPrice.Text, out decimal num) == false)
            {
                return false;
            }
            // Checks if the combo box has an option selected and is not still on blank.
            if (cboCategory.SelectedIndex < 0)
            {
                return false;
            }
            // Checks if the combo box has an option selected and is not still on blank.
            if (cboUsers.SelectedIndex < 0)
            {
                return false;
            }
            // Checks if the date picker has a date selected which is not in the future.
            if (pkrDate.SelectedDate == null || pkrDate.SelectedDate > DateTime.Today)
            {
                return false;
            }
            return true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Checks if the form is filled correctly. If not, alert the user and cancel further action.
            if (IsFormFilledCorrectly() == false)
            {
                MessageBox.Show("Please fill form correctly before saving.\n " +
                                "*All fields must be filled/selected.\n" +
                                "*Price must be in numeric format only (eg. 19.99).\n" +
                                "*Date selected must not be in the future.") ;
                return;
            }
            // Collect all the form data and put it into a data object for saving.
            Expense currentExpense = new Expense();
            currentExpense.Price = decimal.Parse(txtPrice.Text);
            currentExpense.Date = pkrDate.SelectedDate.Value;
            currentExpense.UserId = (int)cboUsers.SelectedValue;
            currentExpense.CategoryId = (int)cboCategory.SelectedValue;
            // Save the data based upon whether it is new data or an update to an existing record. 
            if (isNewEntry)
            {
                data.SaveNewExpense(currentExpense);
            }
            else 
            {
                currentExpense.Id = int.Parse(txtId.Text);
                data.UpdateExpense(currentExpense);
            }
            // Update UI components to display current datsa state.
            UpdateDataGrid();
            ClearDataEntryFields();
        }

        

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearDataEntryFields();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            //Checks that a valid row is selected, otherwise it returns out of the method.
            if (dgvExpenses.SelectedIndex < 0)
            {
                return;
            }
            // Get the Id of the selected entry from the list.
            int Id = expenseList[dgvExpenses.SelectedIndex].Id;
            // Open a message box to confirm deleting the selected entry.
            MessageBoxResult response = MessageBox.Show("Are you sure you want to delete this entry?",
                                                            "Delete Confirmation", MessageBoxButton.YesNo);
            // If the user pressed YES, go ahead with deletion.
            if (response == MessageBoxResult.Yes)
            {
                // Send request to delete entry matching provided Id.
                data.DeleteExpense(Id);
                ClearDataEntryFields();
                UpdateDataGrid();
            }
        }

        private void dgvExpenses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvExpenses.SelectedIndex == -1)
            {
                return;
            }
            // Retrieve the Id (primary key) of the selected row in the table.
            int id = expenseList[dgvExpenses.SelectedIndex].Id;
            // Request the record from the database that matches the provided id. 
            Expense selectedExpense = data.GetExpenseById(id);
            // Set the text fields in the entry form to the matching properties of the model.
            txtId.Text = selectedExpense.Id.ToString();
            txtPrice.Text = selectedExpense.Price.ToString();
            // Set the Selected Value of the combo bnoxes to the entries that match the Id values of their matching
            // properties. 
            // NOTE: Don't use the SeletedIndex value in case the Id and row numbers do not match up.
            cboCategory.SelectedValue = selectedExpense.CategoryId;
            cboUsers.SelectedValue = selectedExpense.UserId;
            // Set the datatime picker to match the value of the data property of the data model.
            pkrDate.SelectedDate = selectedExpense.Date;

            isNewEntry = false;
        }
    }
}
