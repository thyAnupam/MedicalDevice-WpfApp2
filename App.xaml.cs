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
using log4net;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        IUnityContainer container; // = new UnityContainer();
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        public App()
        {
            container = new UnityContainer();
            container.RegisterType<MoDbContext>();
            //context = container.Resolve<MoDbContext>();
        }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Info("        =============  Started Application  =============        ");
            base.OnStartup(e);
            Startup += App_Startup;
            
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            var mainW = new MainWindow(container);
            mainW.Show();
        }
    }
}
