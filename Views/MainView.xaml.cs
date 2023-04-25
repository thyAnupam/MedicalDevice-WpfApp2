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
using System.Windows.Shapes;
using WpfApp2.Repository;
using WpfApp2.Repository.Models;
using WpfApp2.ViewModels.Services;

namespace WpfApp2.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Page, INavigationService
    {
        User _user;
        public MainView(User user)
        {
            InitializeComponent();
            
            _user = user;

        }
        public void NavigateTo(object page)
        {
            ContentArea.Content = page;

        }

        private void SearchPatientButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddPatientButton_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new AddPatientView();

            (ContentArea.Content as AddPatientView).PatientAdded += (sender, e) =>
            {
                ContentArea.Content = new AddDataToPatientUserControl();
            };

        }

        private void UserManagementButton_Click(object sender, RoutedEventArgs e)
        {
            if(_user.RoleId == 1)
            {
                ContentArea.Content = new AdminView(_user);
            }
            else
            {
                ContentArea.Content = new NonAdminView(_user);
            }

        }

        
    }
}
