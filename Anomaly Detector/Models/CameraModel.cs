using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Anomaly_Detector.Models
{
    /// <summary>
    /// Represents a camera configuration, including properties such as the camera's index, 
    /// IP address, port, description, threshold, and connection status.
    /// It implements INotifyPropertyChanged to notify the UI of any property changes.
    /// This class is used for managing individual camera configurations in the anomaly detection system.
    /// </summary>
    public class CameraModel : INotifyPropertyChanged
    {
        // Unique identifier for the camera, used to track each camera in the system
        public int CameraIndex { get; set; }

        // IP address of the camera, used for network communication
        public string? IPAddress { get; set; }

        // Port number for communication with the camera
        public int? Port { get; set; }

        // Description of the camera, usually shown in the UI
        public string? Description { get; set; } = "New Camera";

        // Property to store the threshold value for anomaly detection, 
        // used to compare camera frames or sensor data for detecting anomalies
        private double _threshold;
        public double Threshold
        {
            get => _threshold;
            set { _threshold = value; OnPropertyChanged(nameof(Threshold)); }
        }

        // Flag to indicate whether the camera is currently connected or not
        // This property helps the UI show the connection status of the camera
        private bool _isConnected;

        

        public bool IsConnected
        {
            get => _isConnected;
            set { _isConnected = value; OnPropertyChanged(nameof(IsConnected)); }
        }

        // Arrays of ImageModel objects for target and standard images
        public ImageModel[] TargetImages { get; set; }
        public ImageModel[] StandardImages { get; set; }

        public CameraModel()
        {
            TargetImages = new ImageModel[0];
            StandardImages = new ImageModel[0];
        }

        // Event to notify the UI about property changes, ensuring the UI is updated when properties change
        public event PropertyChangedEventHandler? PropertyChanged;

        // Method to raise the PropertyChanged event for a given property name
        // This method ensures that when a property changes, the UI is notified and can refresh accordingly
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
