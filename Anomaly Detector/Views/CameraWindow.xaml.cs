using System;
using System.Windows;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Windows.Media.Imaging;
using Anomaly_Detector.Models;
using Anomaly_Detector.Services;
using System.Windows.Threading;

namespace Anomaly_Detector.Views
{
    public partial class CameraWindow : Window
    {
        private VideoCapture _camera;
        private string _storagePath;

        public CameraWindow(string cameraIndex, string storagePath)
        {
            InitializeComponent();
            _storagePath = storagePath;
            _camera = new VideoCapture(int.Parse(cameraIndex));
            StartCameraFeed();
        }

        private void StartCameraFeed()
        {
            // Create a background worker or use async to fetch frames
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += (s, e) =>
            {
                var frame = _camera.QueryFrame();
                if (frame != null)
                {
                    CameraFeedImage.Source = frame.ToBitmapSource();
                }
            };
            timer.Start();
        }

        private void CaptureButton_Click(object sender, RoutedEventArgs e)
        {
            var frame = _camera.QueryFrame();
            if (frame != null)
            {
                var imagePath = Path.Combine(_storagePath, "Standard", $"{DateTime.Now:yyyyMMdd_HHmmss}.jpg");
                frame.Save(imagePath);
                UpdateSettingsJson(imagePath);
                MessageBox.Show($"Image saved at {imagePath}");
            }
        }

        private void UpdateSettingsJson(string imagePath)
        {
            // Assuming ApplicationSettings holds your current settings
            var jsonService = new JsonDatabaseService<ApplicationSettings>("settings.json");
            ApplicationSettings settings = jsonService.LoadData();
            jsonService.SaveData(settings);
        }
    }
}
