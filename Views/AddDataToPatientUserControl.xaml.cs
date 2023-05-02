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
using BasicAudio;
using DirectShowLib;
using FFMpegCore;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;



namespace WpfApp2.Views
{
    /// <summary>
    /// Interaction logic for AddDataToPatientUserControl.xaml
    /// </summary>
    public partial class AddDataToPatientUserControl : UserControl
    {
        //private readonly Webcam webcam;
        bool isCameraRunning = false;
        bool isMicrophoneJustStarted = false;
        VideoCapture capture;
        VideoWriter outputVideo;
        Recording audioRecorder;

        Mat frame;
        Bitmap imageAlternate;
        Bitmap image;
        bool isUsingImageAlternate = false;
        DispatcherTimer recordingTimer;

        public AddDataToPatientUserControl()
        {
            InitializeComponent();
            var videoDevices = new List<DsDevice>(DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice));
            foreach (var device in videoDevices)
            {
                ddlVideoDevices.Items.Add(device.Name);
            }
            recordingTimer = new DispatcherTimer();
            recordingTimer.Tick += recordingTimer_Tick;
            //dispatcherTimer.Interval = newTimeSpan(0, 0, 1);
            //dispatcherTimer.Start();
        }


        private async void btnRecord_Click(object sender, EventArgs e)
        {
            if (ddlVideoDevices.SelectedIndex < 0)
            {
                MessageBox.Show("Please choose a video device as the Video Source.", "Video Source Not Defined", MessageBoxButton.OK);
                return;
            }

            if (!isCameraRunning)
            {
                lblStatus.Content = "Starting recording...";

                StartCamera();
                StartMicrophone();

                recordingTimer.IsEnabled = true;
                recordingTimer.Start();

                lblStatus.Content = "Recording...";
            }
            else
            {
                StopCamera();
                StopMicrophone();

                lblStatus.Content = "Recording ended.";
                

                await OutputRecordingAsync();
            }
        }

        private void StartCamera()
        {
            DisposeCameraResources();

            isCameraRunning = true;

            recordButton.Content = "Stop";


            int deviceIndex = ddlVideoDevices.SelectedIndex;
            capture = new VideoCapture(deviceIndex);
            capture.Open(deviceIndex);

            outputVideo = new VideoWriter("video.mp4", FourCC.AVC, 29, new OpenCvSharp.Size(640, 480));
        }
        private void StopCamera()
        {
            isCameraRunning = false;

            recordButton.Content = "Start";

            recordingTimer.Stop();
            recordingTimer.IsEnabled = false;

            DisposeCaptureResources();
        }

        private void StartMicrophone()
        {
            audioRecorder = new Recording();
            audioRecorder.Filename = "sound.wav";
            isMicrophoneJustStarted = true;
        }

        private void StopMicrophone()
        {
            audioRecorder.StopRecording();
            isMicrophoneJustStarted = false;
        }

        private async Task OutputRecordingAsync()
        {
            string outputPath = $"output_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.mp4";

            try
            {

                if (outputVideo == null)
                {
                    lblStatus.Content = "Null recording. Please record again";

                }
                else
                {
                    FFMpeg.ReplaceAudio("video.mp4", "sound.wav", outputPath, true);

                    //VideoRepository videoRepository = new VideoRepository();
                    //string result = videoRepository.SaveVideo(outputVideo);


                    lblStatus.Content = $"Recording saved to local disk with the file name {outputPath}.";

                }



            }
            catch (Exception ex)
            {
                lblStatus.Content = "Recording cannot be saved.";

                MessageBox.Show($"Recording cannot be saved because {ex.Message}", "Error on Recording Saving", MessageBoxButton.OK);
            }
        }

        private void DisposeCameraResources()
        {
            if (frame != null)
            {
                frame.Dispose();
            }

            if (image != null)
            {
                image.Dispose();
            }

            if (imageAlternate != null)
            {
                imageAlternate.Dispose();
            }
        }

        private void DisposeCaptureResources()
        {
            if (capture != null)
            {
                capture.Release();
                capture.Dispose();
            }

            if (outputVideo != null)
            {
                outputVideo.Release();
                outputVideo.Dispose();
            }
        }
        private BitmapImage BitmapToImageSource(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }

        private void recordingTimer_Tick(object sender, EventArgs e)
        {
            if (capture.IsOpened())
            {
                try
                {
                    frame = new Mat();
                    capture.Read(frame);
                    if (frame != null)
                    {
                        if (imageAlternate == null)
                        {
                            isUsingImageAlternate = true;
                            imageAlternate = BitmapConverter.ToBitmap(frame);
                        }
                        else if (image == null)
                        {
                            isUsingImageAlternate = false;
                            image = BitmapConverter.ToBitmap(frame);
                            
                        }

                        img.Source = isUsingImageAlternate ? BitmapToImageSource(imageAlternate) : BitmapToImageSource(image);

                        outputVideo.Write(frame);
                    }
                }
                catch (Exception)
                {
                    img.Source = null;
                }
                finally
                {
                    if (frame != null)
                    {
                        frame.Dispose();
                    }

                    if (isUsingImageAlternate && image != null)
                    {
                        image.Dispose();
                        image = null;
                    }
                    else if (!isUsingImageAlternate && imageAlternate != null)
                    {
                        imageAlternate.Dispose();
                        imageAlternate = null;
                    }
                }

                if (isMicrophoneJustStarted)
                {
                    audioRecorder.StartRecording();
                    isMicrophoneJustStarted = false;
                }
            }
        }

            

    }
}

































