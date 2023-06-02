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
using Unity;
using WpfApp2.Repository;
using WpfApp2.ViewModels;
using WpfApp2.ViewModels.Services;
using WpfApp2.Views;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INavigationService
    {
        MoDbContext _moDbContext;
        IUnityContainer container;
        public MainWindow(IUnityContainer container)
        {
            InitializeComponent();
            
            _moDbContext = container.Resolve<MoDbContext>();
            MainFrame.Content = new LoginView(_moDbContext, this);  ///Uri("Views/LoginView.xaml", UriKind.RelativeOrAbsolute);   
        }

        public void NavigateTo(object page)
        {
            MainFrame.Content = page;
        }
    }
}
