using Anomaly_Detector.Models;
using Anomaly_Detector.Services;
using Emgu.CV;

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Anomaly_Detector.ViewModels
{
    /// <summary>
    /// The main view model for the application, exposing the settings and commands for image capture and comparison.
    /// </summary>
    public class MonitoringViewModel : BaseViewModel
    {
        public ObservableCollection<CameraModel> Cameras { get; set; }

        public ICommand CaptureImageCommand { get; set; }
        public ICommand CompareImagesCommand { get; set; }

        public MonitoringViewModel()
        {
            Cameras = LoadCameras();
            CaptureImageCommand = new RelayCommand(CaptureImage);
            CompareImagesCommand = new RelayCommand(CompareImages);
        }

        private ObservableCollection<CameraModel> LoadCameras()
        {
            var jsonDatabaseService = new JsonDatabaseService<ApplicationSettings>("settings.json");
            var settings = jsonDatabaseService.LoadData();
            return settings?.Cameras ?? new ObservableCollection<CameraModel>();
        }

        private void CaptureImage(object parameter)
        {
            if (parameter is CameraModel cam)
            {
                using (var cameraService = new CameraService(cam.CameraIndex))
                {
                    var frame = cameraService.GetCurrentFrame();
                    if (frame != null)
                    {
                        //cam.TargetImage = frame.ToBitmapSource();
                    }
                }
            }
        }

        private void CompareImages(object parameter)
        {
            if (parameter is CameraModel cam)
            {
                if (cam.TargetImage != null && !string.IsNullOrEmpty(cam.StandardImagePath))
                {
                    //var comparisonResult = ImageComparison.ComputeMeanSquaredError(
                    //    cam.StandardImage, cam.TargetImage);

                    //cam.ComparisonMessage = comparisonResult < cam.Threshold ?
                    //    "Images match" : "Images differ";
                }
            }
        }
    }

}
