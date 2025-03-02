using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Anomaly_Detector.Models
{
    /// <summary>
    /// Represents a camera configuration including its standard image, target image, threshold, and comparison message.
    /// </summary>
    public class CameraModel : INotifyPropertyChanged
    {
        public int CameraIndex { get; set; }
        public string? IPAddress { get; set; }
        public int? Port { get; set; }
        public string? Description { get; set; }
        public string StandardImagePath { get; set; } = string.Empty;

        private BitmapSource _standardImage;
        public BitmapSource StandardImage
        {
            get => _standardImage;
            set { _standardImage = value; OnPropertyChanged(nameof(StandardImage)); }
        }

        private BitmapSource _targetImage;
        public BitmapSource TargetImage
        {
            get => _targetImage;
            set { _targetImage = value; OnPropertyChanged(nameof(TargetImage)); }
        }

        private double _threshold;
        public double Threshold
        {
            get => _threshold;
            set { _threshold = value; OnPropertyChanged(nameof(Threshold)); }
        }

        private string _comparisonMessage;
        public string ComparisonMessage
        {
            get => _comparisonMessage;
            set { _comparisonMessage = value; OnPropertyChanged(nameof(ComparisonMessage)); }
        }

        // New Property: Connection Status Color for Camera
        private string _connectionColor = "Gray";  // Default to gray (disconnected)
        public string ConnectionColor
        {
            get => _connectionColor;
            set { _connectionColor = value; OnPropertyChanged(nameof(ConnectionColor)); }
        }

        // New Property: Flag to indicate if the camera is connected
        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            set { _isConnected = value; OnPropertyChanged(nameof(IsConnected)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
