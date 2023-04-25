using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace WpfApp2.Views
{
    /// <summary>
    /// Interaction logic for AddDataToPatientUserControl.xaml
    /// </summary>
    public partial class AddDataToPatientUserControl : UserControl
    {
        //private readonly Webcam webcam;
        private readonly DispatcherTimer timer;
        private bool isRecording = false;
        public AddDataToPatientUserControl()
        {
            InitializeComponent();

            // Create a new instance of the Webcam class
            //webcam = new Webcam();

            // Set the MediaElement source to the Webcam's VideoBrush
            //mediaElement.Source = webcam.VideoBrush;

            // Create a new timer for updating the MediaElement
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1 / 30.0);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the MediaElement's layout
            mediaElement.InvalidateVisual();
            mediaElement.UpdateLayout();
        }

        private void recordButton_Click(object sender, RoutedEventArgs e)
        {
            if (isRecording)
            {
                // Stop recording and save the video
                string fileName = $"video_{DateTime.Now:yyyyMMddHHmmss}.avi";
                //webcam.StopRecording(fileName);
                isRecording = false;
                recordButton.Content = "Record";
            }
            else
            {
                // Start recording
                //webcam.StartRecording();
                isRecording = true;
                recordButton.Content = "Stop";
            }

        }

        private void captureButton_Click(object sender, RoutedEventArgs e)
        {
            // Capture a frame from the webcam and save it as a PNG image
            string fileName = $"image_{DateTime.Now:yyyyMMddHHmmss}.png";
            //SaveImage(webcam.CaptureImage(), fileName);

        }

        private void SaveImage(ImageSource imageSource, string fileName)
        {
            // Save the image to a file
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageSource as BitmapSource));
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }
    }
}

