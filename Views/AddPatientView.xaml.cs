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
using WpfApp2.Repository.Models;

namespace WpfApp2.Views
{
    /// <summary>
    /// Interaction logic for AddPatientView.xaml
    /// </summary>
    public partial class AddPatientView : UserControl
    {
        public event EventHandler PatientAdded;
        
        //public event EventHandler<User> u;

        public AddPatientView()
        {
            InitializeComponent();
            
        }

        private void AddPatientButton_Click(object sender, RoutedEventArgs e)
        {

            //Take inputs
            //Create patient and add patient to context
            //Save changes
            //Pass patient id to AddData UserControl

            
            PatientAdded?.Invoke(this, EventArgs.Empty);
        }
    }
}
