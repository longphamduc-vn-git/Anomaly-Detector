using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Anomaly_Detector.Models;
using Anomaly_Detector.Services;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Anomaly_Detector.Views
{
    public partial class CameraWindow : Window
    {
        private CameraModel _camera;
        private CameraService _cameraService;
        private BitmapImage _capturedImageBitmap;
        public ApplicationSettings ApplicationSettings { get; set; }

        public CameraWindow(CameraModel camera)
        {
            InitializeComponent();
            _camera = camera;
            _cameraService = new CameraService(camera.CameraIndex);  // Initialize the camera service
        }

        // Window Loaded event handler to start capturing when the window opens
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartCamera();  // Start the camera when the window is loaded
        }

        // Starts the camera feed and begins capturing frames
        private async void StartCamera()
        {
            var frame = _cameraService.GetCurrentFrame();
            if (frame != null)
            {
                CapturedImage.Source = frame.ToBitmapSource();
            }

            // Loop to continuously update the camera feed
            while (true)
            {
                frame = _cameraService.GetCurrentFrame();
                if (frame != null)
                {
                    CapturedImage.Source = frame.ToBitmapSource();
                }
                await Task.Delay(30); // Adjust delay for frame rate
            }
        }

        // Capture the image from the camera and display it
        private void CaptureButton_Click(object sender, RoutedEventArgs e)
        {
            var frame = _cameraService.GetCurrentFrame();
            if (frame != null)
            {
                CapturedImage.Source = frame.ToBitmapSource();

            }
        }

        // Save the captured image to the Standard directory
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_capturedImageBitmap != null)
            {
                string directoryPath = Path.Combine(_camera.StandardImagePath, "Standard");
                string filePath = Path.Combine(directoryPath, "standard_image.jpg");

                // Ensure the directory exists
                Directory.CreateDirectory(directoryPath);

                // Save the image as a .jpg file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    BitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(_capturedImageBitmap));
                    encoder.Save(stream);
                }

                // Update the StandardImagePath in the CameraModel
                _camera.StandardImagePath = filePath;

                // Optionally, save the settings to file if needed
                var jsonService = new JsonDatabaseService<ApplicationSettings>("settings.json");
                jsonService.SaveData(ApplicationSettings);

                MessageBox.Show("Image saved successfully!");
            }
            else
            {
                MessageBox.Show("No image captured.");
            }
        }

        // Delete the saved image
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_camera.StandardImagePath) && File.Exists(_camera.StandardImagePath))
            {
                try
                {
                    File.Delete(_camera.StandardImagePath);
                    _camera.StandardImagePath = string.Empty;  // Clear the path

                    // Optionally, save settings to file
                    var jsonService = new JsonDatabaseService<ApplicationSettings>("settings.json");
                    jsonService.SaveData(ApplicationSettings);

                    MessageBox.Show("Image deleted successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting image: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("No image to delete.");
            }
        }

        // Ensure the camera service is released when the window is closed
        private void Window_Closed(object sender, EventArgs e)
        {
            _cameraService.Release(); // Release the camera resources
        }
    }
}
