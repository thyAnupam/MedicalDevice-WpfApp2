using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using WpfApp2.Repository;
using WpfApp2.Repository.Models;
using WpfApp2.ViewModels.Services;
using static Azure.Core.HttpHeader;
using System.ComponentModel;
using System.Windows.Media.Animation;
using WpfApp2.Resources;

namespace WpfApp2.ViewModels
{
    internal class AdminViewModel : ViewModelBase
    {
        private string _role;
        private string _errorMessage;
        private MoDbContext _context;
        private User _user;
        //----->Add User Details below----------------------
        private string _username="";
        private string _password="";
        private string _email = "";
        private string _firstname = "";
        private string _lastname = "";
        private string _usergroup = "";



        public AdminViewModel(MoDbContext context, User user) //IAuthService authService
        {
            
            _context = context;
            AddUserCommand = new RelayCommand(AddUser);
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

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        //----------------------------------------------------------------------------------------------------------------------


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
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string FirstName
        {
            get => _firstname;
            set => SetProperty(ref _firstname, value);
        }

        public string LastName
        {
            get => _lastname;
            set => SetProperty(ref _lastname, value);
        }

        public string UserGroup
        {
            get => _usergroup;
            set => SetProperty(ref _usergroup, value);
        }




        public ICommand AddUserCommand { get; }

        private void AddUser()
        {

            var uname = Username;
            var fname = FirstName;
            var lname = LastName;
            var pass = Password;
            var email = Email;
            var userGroup = UserGroup;

            if (uname == "" || fname == "" || lname == "" || pass == "" || email == "" || userGroup == "")
            {
                ErrorMessage = Resource1.ProvideAllUserDetails;
            }

            else
            {
                User user = new User { Username = uname, FirstName = fname, LastName = lname, Password = pass, Email = email, RoleId = GetRole(UserGroup)};

                _context.Users.AddAsync(user);
                _context.SaveChanges();

                Username = "";
                FirstName = "";
                LastName = "";
                Password = "";
                Email = "";
                UserGroup = "";

                ErrorMessage = Resource1.UserAdded;            
            }

        }

        public int GetRole(string groupName)
        {
            if (groupName == "Administrator") { return 1; }
            if (groupName == "Doctor") { return 2; }
            if (groupName == "Operator") { return 3; }

            return 0;

        }
    }
}
