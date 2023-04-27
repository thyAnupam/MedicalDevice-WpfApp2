using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp2.Repository.Models;
using WpfApp2.Repository;

namespace WpfApp2.ViewModels
{
    public class NonAdminViewModel: ViewModelBase
    {

        private string _role;
        private User _user;

        //----->Add User Details below----------------------
        
        public NonAdminViewModel(User user) //IAuthService authService
        {
            _user = user;

            if (user.RoleId == 1) { _role = "Administrator"; }
            if (user.RoleId == 2) { _role = "Doctor"; }
            if (user.RoleId == 3) { _role = "Operator"; }
        }

        public string CurrentUsername
        {
            get => _user.Username;  //_username;
            //set => SetProperty(ref _username, value);
        }

        public string Role
        {
            get => _role;
        }

        
    }
}
