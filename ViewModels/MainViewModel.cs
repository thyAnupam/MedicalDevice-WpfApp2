using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp2.Repository;
using WpfApp2.Repository.Models;
using WpfApp2.ViewModels.Services;
using WpfApp2.Views;

namespace WpfApp2.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private MoDbContext _context;
        private User _user;
        private readonly INavigationService _mainWindowNavigation;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MainViewModel(MoDbContext context, INavigationService navigationService, User user, INavigationService m) //IAuthService authService
        {
            _navigationService = navigationService;
            _context = context;
            _mainWindowNavigation = m;
            SearchPatientCommand = new RelayCommand(ShowSearch);
            AddPatientCommand = new RelayCommand(ShowAddPatient);
            UserManagementCommand = new RelayCommand(ShowUserManagement);
            LogOutCommand = new RelayCommand(Logout);
            _user = user;
            log4net.Config.XmlConfigurator.Configure();
        }

        public ICommand SearchPatientCommand { get; }
        public ICommand AddPatientCommand { get; }
        public ICommand UserManagementCommand { get; }

        public ICommand LogOutCommand { get; }

        public void ShowSearch()
        {

            _navigationService.NavigateTo(new SearchPatientView(_navigationService));

        }

        public void ShowAddPatient()
        {
            AddPatientView addPatientView = new AddPatientView(_navigationService);
            _navigationService.NavigateTo(addPatientView);


        }

        public void Logout()
        {
            LoginView newlogin = new LoginView(_context, _mainWindowNavigation);
            _mainWindowNavigation.NavigateTo(newlogin);
            log.Info("User logged out");
        }

        public void ShowUserManagement()
        {
            if (_user.RoleId == 1)
            {
                _navigationService.NavigateTo(new AdminView(_user));
            }
            else
            {
                _navigationService.NavigateTo(new NonAdminView(_user));
            }

        }



    }
}
