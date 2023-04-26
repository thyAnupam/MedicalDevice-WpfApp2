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
using WpfApp2.ViewModels;
using WpfApp2.ViewModels.Services;

namespace WpfApp2.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Page, INavigationService
    {
        User _user;
        MoDbContext _dbContext;
        public MainView(MoDbContext context, User user)
        {
            InitializeComponent();
            _user = user;
            _dbContext = context;
            DataContext = new MainViewModel(_dbContext, this, _user);

        }
        public void NavigateTo(object page)
        {
            ContentArea.Content = page;

        }

        
        
    }
}
