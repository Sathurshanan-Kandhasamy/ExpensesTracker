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

namespace ExpensesTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            conMain.Content = new ExpensePanel();
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            conMain.Content = new UsersPanel();
        }

        private void btnExpenses_Click(object sender, RoutedEventArgs e)
        {
            conMain.Content = new ExpensePanel();
        }

        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            conMain.Content = new ReportsPanel();
        }
    }
}
