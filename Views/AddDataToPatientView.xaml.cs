using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using BasicAudio;
using DirectShowLib;
using FFMpegCore;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;
using WpfApp2.ViewModels;

namespace WpfApp2.Views
{
    /// <summary>
    /// Interaction logic for AddDataToPatientUserControl.xaml
    /// </summary>
    public partial class AddDataToPatientView : UserControl
    {
        

        public AddDataToPatientView()
        {
            InitializeComponent();
            DataContext = new AddDataToPatientViewModel();
            
        }


        

            

    }
}

































