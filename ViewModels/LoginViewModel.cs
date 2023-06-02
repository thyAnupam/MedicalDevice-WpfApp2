using System.Windows.Input;
using WpfApp2.ViewModels;
using WpfApp2.Repository;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using WpfApp2.Repository.Models;
using WpfApp2.Resources;
using System.Windows.Navigation;
using WpfApp2.Views;
using Microsoft.EntityFrameworkCore.Metadata;
using WpfApp2.ViewModels.Services;
using log4net;
using System.Linq;

namespace WpfApp2.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {

        private readonly INavigationService _navigationService;
        private string _username;
        private string _password;
        private string _errorMessage;
        private MoDbContext _context;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LoginViewModel(MoDbContext context, INavigationService navigationService) //IAuthService authService
        {
            _navigationService = navigationService;
            _context = context;
            LoginCommand = new RelayCommand(Login);
            log4net.Config.XmlConfigurator.Configure();
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));

            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoginCommand { get; }

        public void Login()
        {
            ErrorMessage = "Loading...";
            var userfound =  _context.Users.Any(u => u.Username == Username && u.Password == Password); ///_authService.AuthenticateAsync(Username, Password);
            if (userfound)
            {
                User curr =  _context.Users.FirstOrDefault(u => u.Username == Username && u.Password == Password);

                log.Info($"User {curr.Username} logged in");
                GrantAccess(new User { FirstName = curr.FirstName, LastName = curr.LastName, Email = curr.Email, Username = curr.Username, RoleId = curr.RoleId });
                
            }

            
            ErrorMessage = Resource1.UserNotFound;
            log.Error($"Login failed");
            


            return;
           
        }

        public void GrantAccess(User u)
        {
            MainView m = new MainView(_context, u, _navigationService);
            _navigationService.NavigateTo(m);

        }
    }
}












//NavigationService.Navigate(m);

//AdminView m = new AdminView(u);
//this.Navigate(m);
//this.NavigationService.Navigate(m);
//this.NavigationService.RemoveBackEntry();
//Navigate(m);
