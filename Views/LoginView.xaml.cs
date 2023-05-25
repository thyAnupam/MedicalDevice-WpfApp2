using System.Windows.Controls;
using System.Windows.Navigation;
using WpfApp2.Repository;
using WpfApp2.Repository.Models;
using WpfApp2.Resources;
using WpfApp2.ViewModels;
using WpfApp2.ViewModels.Services;


namespace WpfApp2.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Page
    {
        
        MoDbContext context;

        public LoginView(MoDbContext _context, INavigationService n)
        {
            InitializeComponent();
            context = _context;               
            DataContext = new LoginViewModel(context, n);  
        }
        

    }
}
