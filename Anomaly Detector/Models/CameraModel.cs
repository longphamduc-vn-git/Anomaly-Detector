using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace Anomaly_Detector.Models
{
    /// <summary>
    /// Represents a camera configuration.
    /// </summary>
    public class CameraModel : INotifyPropertyChanged
    {
        private string _filePath;
        private double _threshold;
        private bool _isConnected;

        // Arrays of ImageModel objects for target and standard images
        public ImageModel[] TargetImages { get; set; }
        public ImageModel[] StandardImages { get; set; }

        public int CameraIndex { get; set; }
        public string? IPAddress { get; set; }
        public int? Port { get; set; }
        public string? Description { get; set; } = string.Empty;

        // Threshold for anomaly detection (e.g., used for comparing frames)
        public double Threshold
        {
            get => _threshold;
            set
            {
                _threshold = value;
                OnPropertyChanged(nameof(Threshold));
            }
        }

        // Connection status flag for the camera
        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                _isConnected = value;
                OnPropertyChanged(nameof(IsConnected));
            }
        }

        // Timestamp of when the camera or its images were captured or processed
        public DateTime Timestamp { get; set; } = DateTime.Now;

        // Event for property change notifications
        public event PropertyChangedEventHandler PropertyChanged;

        // Method to trigger property change notifications
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Constructor to initialize CameraModel with empty arrays for target and standard images
        public CameraModel()
        {
            TargetImages = new ImageModel[0]; // Empty array initially
            StandardImages = new ImageModel[0]; // Empty array initially
        }

        // Add a target image to the TargetImages array
        public void AddTargetImage(string filePath)
        {
            var imageModel = new ImageModel { FilePath = filePath };
            //AddImageToArray(ref TargetImages, imageModel);
        }

        // Add a standard image to the StandardImages array
        public void AddStandardImage(string filePath)
        {
            var imageModel = new ImageModel { FilePath = filePath };
            //AddImageToArray(ref StandardImages, imageModel);
        }

        // Helper method to add an image to the image array
        private void AddImageToArray(ref ImageModel[] imageArray, ImageModel imageModel)
        {
            var newArray = new ImageModel[imageArray.Length + 1];
            Array.Copy(imageArray, newArray, imageArray.Length);
            newArray[imageArray.Length] = imageModel;
            imageArray = newArray;
        }
    }
}
