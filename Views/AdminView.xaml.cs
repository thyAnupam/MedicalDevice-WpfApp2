using Microsoft.EntityFrameworkCore;
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
using System.Resources;
using WpfApp2.Repository.Models;
using WpfApp2.Resources;

namespace WpfApp2.Views
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : UserControl
    {
        MoDbContext _db;
        public AdminView(User user)
        {
            InitializeComponent();
            _db = new MoDbContext();


            curr_user.Text = user.Username;
            int roleid = user.RoleId;
            if(roleid == 1) { curr_user_group.Text = "Administrator"; }
            if (roleid == 2) { curr_user_group.Text = "Doctor"; }
            if (roleid == 3) { curr_user_group.Text = "Operator"; }
            //curr_user_group.Text = context.Users.FirstOrDefault(u => u.Username == uname);
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {

            var uname = usernameTextBox.Text;
            var fname = fnameTextBox.Text;
            var lname = lnameTextBox.Text;
            var pass = passTextBox.Text;
            var email = mailTextBox.Text;
            var userGroup = combo.Text;

            if(uname =="" || fname == "" || lname == "" || pass == "" || email=="" || userGroup == "")
            {
                SBar.Items.Clear();
                TextBox tb = new TextBox();
                tb.Text = "Please enter everything";
                SBar.Items.Add(tb);
            }

            else
            {
                User user = new User { Username=uname, FirstName=fname, LastName=lname, Password=pass, Email=email, RoleId = combo.SelectedIndex+1};

                
                _db.AddAsync(user);
                _db.SaveChanges();


                SBar.Items.Clear();
                TextBox tb = new TextBox();
                tb.Text = "User added";
                SBar.Items.Add(tb);

            }
            
        }
    }
}
