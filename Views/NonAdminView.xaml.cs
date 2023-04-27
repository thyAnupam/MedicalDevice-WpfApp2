﻿using System;
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
using WpfApp2.Repository.Models;
using WpfApp2.ViewModels;


namespace WpfApp2.Views
{
    /// <summary>
    /// Interaction logic for NonAdminView.xaml
    /// </summary>
    public partial class NonAdminView : UserControl
    {
        public NonAdminView(User user)
        {
            InitializeComponent();
            DataContext = new NonAdminViewModel(user);

        }
    }
}
