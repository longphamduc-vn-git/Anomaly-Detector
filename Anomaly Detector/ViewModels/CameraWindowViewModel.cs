using Anomaly_Detector.Models;
using Anomaly_Detector.Services;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Threading.Tasks;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Windows.Media;
using System.Windows;
using System.IO;

namespace Anomaly_Detector.ViewModels
{
    public class CameraWindowViewModel : BaseViewModel
    {
        private CameraService _cameraService;
        private ImageSource _cameraFeedImage;
        private string _capturedImagePath;
        public CameraModel _camera;
        private bool _isCaptured = false;
        public ApplicationSettings ApplicationSettings { get; set; }
        public ICommand CaptureImageCommand { get; set; }
        public ICommand DeleteImageCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }
        private ApplicationSettings LoadSettings()
        {
            var jsonService = new JsonDatabaseService<ApplicationSettings>("settings.json");
            return jsonService.LoadData() ?? new ApplicationSettings();  // Default settings if not found
        }
        private void CloseWindow(object parameter)
        {
            try
            {
                if (parameter is Window window)
                {
                    // Lưu cài đặt
                    var jsonService = new JsonDatabaseService<ApplicationSettings>("settings.json");
                    jsonService.SaveData(ApplicationSettings);

                    window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}");
            }
        }



        public CameraWindowViewModel()
        {
            ApplicationSettings = LoadSettings(); // Load saved settings from a file

            CaptureImageCommand = new RelayCommand(CaptureImage);
            DeleteImageCommand = new RelayCommand(DeleteImage);
            CloseWindowCommand = new RelayCommand(CloseWindow);
        }

        public void InitializeCamera(CameraModel camera)
        {
            _camera = camera;
            _cameraService = new CameraService(camera.CameraIndex);
            StartCameraFeed();
        }

        public ImageSource CameraFeedImage
        {
            get => _cameraFeedImage;
            set
            {
                _cameraFeedImage = value;
                OnPropertyChanged();
            }
        }



        private void SaveImageSourceToFile(ImageSource imageSource, string filePath)
        {
            // Ensure the image source is a BitmapSource
            BitmapSource bitmapSource = imageSource as BitmapSource;
            if (bitmapSource == null)
            {
                throw new ArgumentException("The provided ImageSource is not a BitmapSource.");
            }

            // Encode the image to the desired format (e.g., PNG)
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

            // Save the image to the specified file path
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }


        private async void StartCameraFeed()
        {
            while (true)
            {
                if (!_isCaptured)
                {
                    var frame = await Task.Run(() => _cameraService.GetCurrentFrame());
                    if (frame != null)
                    {
                        CameraFeedImage = frame.ToBitmapSource();
                    }
                }
                await Task.Delay(100);
            }
        }

        private void CaptureImage(object parameter)
        {
            _isCaptured = true;
            string CapturedImagePath = $"{ApplicationSettings.ImageStoragePath}\\{_camera.Description}_{DateTime.Now.Ticks}.jpg";
            SaveImageSourceToFile(CameraFeedImage, CapturedImagePath);
            _isCaptured = false;
        }

        private void DeleteImage(object parameter)
        {
            CameraFeedImage = null; // Clear the display
        }


        #region Command Initialization

        private void InitializeCommands()
        {
            CaptureImageCommand = new RelayCommand(CaptureImage);
            DeleteImageCommand = new RelayCommand(DeleteImage);
        }

        #endregion

    }
}
