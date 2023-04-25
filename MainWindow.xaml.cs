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
using WpfApp2.Repository;
using WpfApp2.ViewModels;
using WpfApp2.Views;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MoDbContext _moDbContext;
        
        public MainWindow(MoDbContext context)
        {
            InitializeComponent();
            
            _moDbContext = context;
            MainFrame.Content = new LoginView(_moDbContext);  ///Uri("Views/LoginView.xaml", UriKind.RelativeOrAbsolute);   
        }


    }
}
