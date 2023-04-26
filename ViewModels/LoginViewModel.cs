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

namespace WpfApp2.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {

        private readonly INavigationService _navigationService;
        private string _username;
        private string _password;
        private string _errorMessage;
        private MoDbContext _context;

        public LoginViewModel(MoDbContext context, INavigationService navigationService) //IAuthService authService
        {
            _navigationService = navigationService;
            _context = context;
            LoginCommand = new RelayCommand(Login);
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoginCommand { get; }

        public async void Login()
        {
            ErrorMessage = "Loading...";
            var userfound = await _context.Users.AnyAsync(u => u.Username == _username && u.Password == _password); ///_authService.AuthenticateAsync(Username, Password);
            if (userfound)
            {
                User curr = await _context.Users.FirstOrDefaultAsync(u => u.Username == _username && u.Password == _password);


                GrantAccess(new User { FirstName = curr.FirstName, LastName = curr.LastName, Email = curr.Email, Username = curr.Username, RoleId = curr.RoleId });

            }

            else
            {
                
                ErrorMessage = Resource1.UserNotFound;

                _username = "";
                _password = "";

            }
        }

        public void GrantAccess(User u)
        {
            MainView m = new MainView(_context, u);
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
