using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace Anomaly_Detector.Models
{
    /// <summary>
    /// Represents an image, including properties such as file path, comparison score, and the loaded image.
    /// </summary>
    public class ImageModel : INotifyPropertyChanged
    {
        private string _filePath;
        private double _comparisonScore;
        private BitmapImage _image;
        private string _comparisonResult;

        /// <summary>
        /// Gets or sets the file path of the image.
        /// </summary>
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
                LoadImage(); // Automatically load the image when the file path is set
            }
        }

        /// <summary>
        /// Gets or sets the comparison score (used for anomaly detection).
        /// </summary>
        public double ComparisonScore
        {
            get => _comparisonScore;
            set
            {
                _comparisonScore = value;
                OnPropertyChanged(nameof(ComparisonScore));
            }
        }

        /// <summary>
        /// Gets or sets the comparison result (e.g., "Normal", "Anomaly").
        /// </summary>
        public string ComparisonResult
        {
            get => _comparisonResult;
            set
            {
                _comparisonResult = value;
                OnPropertyChanged(nameof(ComparisonResult));
            }
        }

        /// <summary>
        /// Gets or sets the loaded image.
        /// </summary>
        public BitmapImage Image
        {
            get => _image;
            private set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        /// <summary>
        /// Loads the image from the file path.
        /// </summary>
        private void LoadImage()
        {
            if (!string.IsNullOrEmpty(FilePath) && File.Exists(FilePath))
            {
                try
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(FilePath, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    Image = bitmap;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading image: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Notifies UI when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
