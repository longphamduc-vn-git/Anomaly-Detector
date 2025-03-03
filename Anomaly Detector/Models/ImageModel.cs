using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace Anomaly_Detector.Models
{
    /// <summary>
    /// Represents an image used in the anomaly detection system.
    /// This class stores image metadata and provides property change notifications.
    /// </summary>
    public class ImageModel : INotifyPropertyChanged
    {
        private string? _filePath;
        private double _comparisonScore;
        private string? _comparisonResult;
        private BitmapImage? _image;
        private bool _isStandardImage;
        private string _title;

        /// <summary>
        /// Gets or sets the file path of the image.
        /// </summary>
        public string? FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
                LoadImage(); // Automatically load image when file path is set
            }
        }

        /// <summary>
        /// Gets the image loaded from the file path.
        /// </summary>
        public BitmapImage? Image
        {
            get => _image;
            private set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        /// <summary>
        /// Gets or sets the timestamp when the image was captured or processed.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;

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
        public string? ComparisonResult
        {
            get => _comparisonResult;
            set
            {
                _comparisonResult = value;
                OnPropertyChanged(nameof(ComparisonResult));
            }
        }

        /// <summary>
        /// Gets or sets a flag to determine if this image is the standard or target image.
        /// </summary>
        public bool IsStandardImage
        {
            get => _isStandardImage;
            set
            {
                _isStandardImage = value;
                OnPropertyChanged(nameof(IsStandardImage));
            }
        }

        /// <summary>
        /// Gets or sets the title or description of the image.
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        /// <summary>
        /// Notifies UI when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
