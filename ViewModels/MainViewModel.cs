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

        public MainViewModel(MoDbContext context, INavigationService navigationService, User user) //IAuthService authService
        {
            _navigationService = navigationService;
            _context = context;
            SearchPatientCommand = new RelayCommand(ShowSearch);
            AddPatientCommand = new RelayCommand(ShowAddPatient);
            UserManagementCommand = new RelayCommand(ShowUserManagement);
            _user = user;

        }

        public ICommand SearchPatientCommand { get; }
        public ICommand AddPatientCommand { get; }
        public ICommand UserManagementCommand { get; }

        public void ShowSearch()
        {

            _navigationService.NavigateTo(new SearchPatientView());

        }

        public void ShowAddPatient()
        {
            AddPatientView addPatientView = new AddPatientView(_navigationService);
            _navigationService.NavigateTo(addPatientView);


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
