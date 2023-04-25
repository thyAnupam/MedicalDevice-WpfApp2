using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfApp2.Repository;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Unity.Lifetime;
using Unity;
using Microsoft.EntityFrameworkCore;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        MoDbContext context;
        IUnityContainer container; // = new UnityContainer();

        public App()
        {
            container = new UnityContainer();
            container.RegisterType<MoDbContext>();
            context = container.Resolve<MoDbContext>();
        }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Startup += App_Startup;
            
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            //context = container.Resolve<MoDbContext>();

            var mainW = new MainWindow(context);
            mainW.Show();
        }
    }
}
