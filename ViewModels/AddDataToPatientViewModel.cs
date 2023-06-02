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
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Windows.Media;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using WpfApp2.Repository;
using WpfApp2.Repository.Models;
using log4net;
using System.Linq;

namespace WpfApp2.ViewModels
{
    internal class AddDataToPatientViewModel: ViewModelBase
    {
        bool isCameraRunning = false;
        bool isMicrophoneJustStarted = false;
        VideoCapture capture;
        VideoWriter outputVideo;
        Recording audioRecorder;
        private int _patientID;
        private int _seriesID;
        private Patient CurrentPatient;

        Mat frame;
        Bitmap imageAlternate;
        Bitmap image;
        bool isUsingImageAlternate = false;
        DispatcherTimer recordingTimer;
        private ObservableCollection<string> _videoDevices=new ObservableCollection<string>();
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ObservableCollection<string> VideoDevices
        {
            get => _videoDevices; 

        }

        private BitmapImage img;

        public BitmapImage IMG
        {
            get => img; set => SetProperty(ref img, value);
        }


        private string selectedDevice;

        public string SelectedDevice
        {
            get => selectedDevice; set => SetProperty(ref selectedDevice, value);
        }

        private string _lblStatus;

        public string LblStatus
        {
            get => _lblStatus;
            set => SetProperty(ref _lblStatus, value);
        }

        private string _buttonText="Record";

        public string ButtonText
        {
            get => _buttonText;
            set => SetProperty(ref _buttonText, value);
        }
        private MoDbContext context;

        public AddDataToPatientViewModel(int patientId, int seriesID)
        {
            _patientID = patientId;
            _seriesID = seriesID;
            context = new MoDbContext();
            
            LoadDevices();
            

            recordingTimer = new DispatcherTimer();
            recordingTimer.Tick += recordingTimer_Tick;

            RecordCommand = new RelayCommand(btnRecord_Click);
            ExportCommand = new RelayCommand(CreateSavePDF);
            CaptureCommand = new RelayCommand(Capture); 
            log4net.Config.XmlConfigurator.Configure();
        }
        public ICommand ExportCommand { get; }
        public ICommand CaptureCommand { get; }
        private void LoadDevices()
        {
            var videoDevices = new List<DsDevice>(DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice));
            foreach (var device in videoDevices)
            {
                _videoDevices.Add(device.Name);
            }

        }

        public ICommand RecordCommand { get; }
        private async void btnRecord_Click()
        {
            if (selectedDevice==null)
            {
                MessageBox.Show("Please choose a video device as the Video Source.", "Video Source Not Defined", MessageBoxButton.OK);
                return;
            }

            if (!isCameraRunning)
            {
                LblStatus = "Starting recording...";

                StartCamera();
                StartMicrophone();

                recordingTimer.IsEnabled = true;
                recordingTimer.Start();

                LblStatus = "Recording...";
            }
            else
            {
                StopCamera();
                StopMicrophone();

                LblStatus = "Recording ended.";
                IMG = null;


                await OutputRecordingAsync();
            }
        }

        private void StartCamera()
        {
            DisposeCameraResources();

            isCameraRunning = true;

            ButtonText = "Stop";


            int deviceIndex = VideoDevices.IndexOf(SelectedDevice);
            capture = new VideoCapture(deviceIndex);
            capture.Open(deviceIndex);

            outputVideo = new VideoWriter("video.mp4", FourCC.AVC, 29, new OpenCvSharp.Size(640, 480));
        }
        private void StopCamera()
        {
            isCameraRunning = false;

            ButtonText = "Start";

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
            string outputPath = $"C:\\Users\\X6AUJWAN\\Desktop\\Videos\\output_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.mp4";

            try
            {

                if (outputVideo == null)
                {
                    LblStatus = "Null recording. Please record again";

                }
                else
                {
                    FFMpeg.ReplaceAudio("video.mp4", "sound.wav", outputPath, true);
                    LblStatus = $"Recording saved to local disk with the file name {outputPath}";
                    var video = new Video {SeriesId=_seriesID, VideoName="oxdohxesh", VideoPath=outputPath };


                    context.Videos.Add(video);
                    context.SaveChanges();
                    log.Info($"Video added to path {outputPath}");


                }



            }
            catch (Exception ex)
            {
                LblStatus = "Recording cannot be saved.";

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


        private void CreateSavePDF()
        {
            Repository.Models.Image RetrievedImage;

            CurrentPatient = context.Patients.FindAsync(_patientID).Result;
            RetrievedImage = context.Images.FirstOrDefault(x => x.SeriesId == _seriesID);


            string Firstname = CurrentPatient.Firstname;
            string Lastname = CurrentPatient.Lastname;
            string gender="Not assigned";
            if (CurrentPatient.Gender == 1)
            {
                gender = "Male";
            }
            if (CurrentPatient.Gender == 2)
            {
                gender = "Female";
            }
            string d = CurrentPatient.Dob.ToString();
            


            // Create a SaveFileDialog to prompt the user for the file save location
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PDF Files (.pdf)|.pdf";
            if (saveDialog.ShowDialog() == true)
            {
                // Get the selected file path from the save dialog
                string filePath = saveDialog.FileName;

                // Create a new PDF document
                PdfDocument document = new PdfDocument();

                // Create a new page
                PdfPage page = document.AddPage();

                // Create a graphics object for the page
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Create a new XFont for the text
                XFont font = new XFont("Arial", 12, XFontStyle.Regular);
                XFont subheadingFont = new XFont("Arial", 8, XFontStyle.BoldItalic);
                // Create a new XImage from the sample image file
                
                

                // Draw "Hello World" text on the page
                gfx.DrawString($"Patient Name: {Firstname} {Lastname}", font, XBrushes.Black, new XPoint(50, 50));
                gfx.DrawString($"Patient DOB: {d}", font, XBrushes.Black, new XPoint(50, 80));
                gfx.DrawString($"Patient Gender: {gender}", font, XBrushes.Black, new XPoint(50, 110));
                gfx.DrawString("Patient Report Exported By: CURRENT_USER", font, XBrushes.Black, new XPoint(50, 140));
                gfx.DrawString("By Carl Zeiss India - CARIn Bangalore ", subheadingFont, XBrushes.Blue, new XPoint(420, 140));

                // Draw the image on the page
                if (RetrievedImage != null)
                {
                    XImage image = XImage.FromFile(RetrievedImage.ImagePath);
                    gfx.DrawImage(image, 50, 200);

                }
                

                // Save the PDF document to the selected file path
                document.Save(filePath);

                // Close the document
                document.Close();
            }

        }

        private void Capture()
        {
            if(isCameraRunning)
            {
                if (imageAlternate != null)
                {
                    var c = imageAlternate;
                    string outputPath = $"C:\\MedicalDevice\\Images\\output_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.jpeg";
                    c.Save(outputPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    var k = new WpfApp2.Repository.Models.Image { SeriesId = _seriesID, ImageName = "oohjhhg", ImagePath = outputPath };

                    
                    context.Images.Add(k);
                    context.SaveChanges();
                    log.Info($"Image added to path {outputPath}");
                    
                }
                else if (image != null)
                {
                    var c = image;
                    string outputPath = $"C:\\MedicalDevice\\Images\\output_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.jpeg";
                    c.Save(outputPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    var g = new WpfApp2.Repository.Models.Image { SeriesId = _seriesID, ImageName = "oohjhhg", ImagePath = outputPath }; //.Replace("\\", "#")


                    context.Images.Add(g);
                    context.SaveChanges();
                    log.Info($"Image added to path {outputPath}"); //.Replace("#", "\\")

                }

            }
            else
            {
                MessageBox.Show("Please start recording.", "Video Source Not Defined", MessageBoxButton.OK);
                return;

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

                        IMG = isUsingImageAlternate ? BitmapToImageSource(imageAlternate) : BitmapToImageSource(image);

                        outputVideo.Write(frame);
                    }
                }
                catch (Exception)
                {
                    IMG = null;
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
