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
    /// Interaction logic for UsersPanel.xaml
    /// </summary>
    public partial class UsersPanel : UserControl
    {
        // Create our class object for communicating with the database. 
        DataAdapter data = new DataAdapter();
        // A list of User objects.
        List<User> userList = new List<User>();
        //Acts as a flag to indicate which way to save our data, as a new entry or an edit.
        bool isNewEntry = true;

        public UsersPanel()
        {
            InitializeComponent();
            UpdateDataGrid();
        }

        private void UpdateDataGrid()
        {
            userList = data.GetAllUsers();
            dgvUsers.ItemsSource = userList;
            dgvUsers.Items.Refresh();
        }

        private void ClearDataEntryFields()
        {
            txtId.Text= string.Empty;
            txtName.Text= string.Empty;
            txtRole.Text= string.Empty;
            //Sets the save flag to new entry mode.
            isNewEntry = true;
        }

        private bool IsFormFilledCorrectly()
        {
            if (String.IsNullOrWhiteSpace(txtName.Text))
            {
                return false;
            }
            if (String.IsNullOrWhiteSpace(txtRole.Text))
            {
                return false;
            }
            return true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (IsFormFilledCorrectly() == false)
            {
                MessageBox.Show("Make sure form fields are filled correctly before trying to save!");
                return;
            }

            // Get the user details from the entry form
            User userEntry = new User();
            userEntry.Name = txtName.Text;
            userEntry.Role = txtRole.Text;

            //Chooses the desired save mode based upon the state of the isNewEntry flag.
            if (isNewEntry)
            {
                //Pass the user details to the database to be added.
                data.AddNewUser(userEntry);
            }
            else
            {
                // Get the user Id from the entry form.
                userEntry.Id = int.Parse(txtId.Text);
                // Pass the user details to the database to be updated.
                data.UpdateUser(userEntry);
            }

            //Update the on-screen display.
            ClearDataEntryFields();
            UpdateDataGrid();
        }

        

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearDataEntryFields();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            //Checks that a vlid row is selected, otherwise it returns out of the method.
            if (dgvUsers.SelectedIndex < 0)
            {
                return;
            }
            //Get the Id of the selected entry from the list.
            int Id = userList[dgvUsers.SelectedIndex].Id;
            // Open a message box to confirm deleting the selected entry.
            MessageBoxResult response = MessageBox.Show("Are you sure you want to delete this entry?",
                                                            "Delete Confirmation", MessageBoxButton.YesNo);
            // If the user pressed YES, go ahead with deletion.
            if (response == MessageBoxResult.Yes) 
            {
                // Send request to delete entry matching provided Id.
                data.DeleteUser(Id);
                ClearDataEntryFields();
                UpdateDataGrid();
            } 
        }

        private void dgvUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Checks that a vlid row is selected, otherwise it returns out of the method.
            if (dgvUsers.SelectedIndex < 0)
            {
                return;
            }
            //Get the Id of the selected entry from the list.
            int Id = userList[dgvUsers.SelectedIndex].Id;
            //Gets the user from the database that matches the current Id value. 
            User userEntry = data.GetUserById(Id);
            //Copy the user details into the form.
            txtId.Text= userEntry.Id.ToString();
            txtName.Text= userEntry.Name;
            txtRole.Text= userEntry.Role;
            //Sets the save flag to edit mode.
            isNewEntry= false;
        }
    }
}
