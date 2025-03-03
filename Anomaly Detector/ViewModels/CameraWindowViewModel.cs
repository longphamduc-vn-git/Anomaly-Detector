using Anomaly_Detector.Models;
using Anomaly_Detector.Services;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Threading.Tasks;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Windows.Media;

namespace Anomaly_Detector.ViewModels
{
    public class CameraWindowViewModel : BaseViewModel
    {
        private CameraService _cameraService;
        private ImageSource _cameraFeedImage;
        private BitmapImage _capturedImage;
        private CameraModel _camera;

        // Commands
        public ICommand CaptureImageCommand { get; private set; }
        public ICommand DeleteImageCommand { get; private set; }

        // Parameterless constructor for XAML instantiation
        public CameraWindowViewModel()
        {
            InitializeCommands();
        }

        // Method to initialize the camera after instantiation
        public void InitializeCamera(CameraModel camera)
        {
            _camera = camera;
            _cameraService = new CameraService(camera.CameraIndex);
            StartCameraFeed();
        }

        #region Properties

        public ImageSource CameraFeedImage
        {
            get => _cameraFeedImage;
            set
            {
                _cameraFeedImage = value;
                OnPropertyChanged(nameof(CameraFeedImage));
            }
        }

        #endregion

        #region Methods

        private async void StartCameraFeed()
        {
            // Continuously capture frames from the camera and display them
            while (true)
            {
                var frame = await Task.Run(() => _cameraService.GetCurrentFrame());
                if (frame != null)
                {
                    CameraFeedImage = frame.ToBitmapSource(); // Display the frame
                }
                await Task.Delay(100); // Add a slight delay for smooth frame rendering
            }
        }

  

        private void CaptureImage(object parameter)
        {
            if (_capturedImage != null)
            {
                var filePath = $"Captured_{DateTime.Now.Ticks}.jpg";
                _cameraService.SaveImage(_capturedImage, filePath); // Save the captured image
                _capturedImage = null; // Reset after capture
                CameraFeedImage = null; // Reset displayed image
            }
        }

        private void DeleteImage(object parameter)
        {
            CameraFeedImage = null; // Clear the display
        }

        #endregion

        #region Command Initialization

        private void InitializeCommands()
        {
            CaptureImageCommand = new RelayCommand(CaptureImage);
            DeleteImageCommand = new RelayCommand(DeleteImage);
        }

        #endregion
    }
}
