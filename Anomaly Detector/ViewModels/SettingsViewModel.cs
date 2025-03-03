using Anomaly_Detector.Models;
using Anomaly_Detector.Services;
using Anomaly_Detector.Views;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Anomaly_Detector.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public ApplicationSettings ApplicationSettings { get; set; }

        // Commands for managing cameras, PLC endpoints, and preprocessing steps.
        public ICommand AddCameraCommand { get; set; }
        public ICommand RegisterPLCEndpointsCommand { get; set; }
        public ICommand AddPreprocessingStepCommand { get; set; }
        public ICommand RemovePreprocessingStepCommand { get; set; }
        public ICommand SortPreprocessingStepsCommand { get; set; }

        // Commands for connectivity and image capture.
        public ICommand CheckCameraConnectionCommand { get; set; }
        public ICommand CheckPLCConnectionCommand { get; set; }
        public ICommand CaptureStandardImageCommand { get; set; }
        public ICommand SaveSettingsCommand { get; set; }
        public ICommand SaveCapturedImageCommand { get; set; }
        public ICommand DeleteCapturedImageCommand { get; set; }

        private string? _capturedImagePath;

        // Property to indicate connectivity.
        private bool _cameraConnected;
        public bool CameraConnected
        {
            get => _cameraConnected;
            set { _cameraConnected = value; OnPropertyChanged(); }
        }

        private bool _plcConnected;
        public bool PLCConnected
        {
            get => _plcConnected;
            set { _plcConnected = value; OnPropertyChanged(); }
        }

        // Property to hold the captured image for display.
        private BitmapSource? _capturedImage; // Make nullable
        public BitmapSource? CapturedImage
        {
            get => _capturedImage;
            set { _capturedImage = value; OnPropertyChanged(); }
        }


        // Property to control visibility of the Capture Standard Image button.
        private bool _showCaptureButton;
        public bool ShowCaptureButton
        {
            get => _showCaptureButton;
            set { _showCaptureButton = value; OnPropertyChanged(); }
        }

        // Initializes a new instance of the SettingsViewModel class.
        public SettingsViewModel()
        {
            ApplicationSettings = LoadSettings();  // Load saved settings

            // Other initialization code...
            ApplicationSettings.Cameras.CollectionChanged += Cameras_CollectionChanged;

            // Initialize commands
            AddCameraCommand = new RelayCommand(AddCamera);
            RegisterPLCEndpointsCommand = new RelayCommand(RegisterPLCEndpoints);
            AddPreprocessingStepCommand = new RelayCommand(AddPreprocessingStep);
            RemovePreprocessingStepCommand = new RelayCommand(RemovePreprocessingStep);
            SortPreprocessingStepsCommand = new RelayCommand(SortPreprocessingSteps);
            CheckCameraConnectionCommand = new RelayCommand(CheckCameraConnection);
            CheckPLCConnectionCommand = new RelayCommand(CheckPLCConnection);
            SaveSettingsCommand = new RelayCommand(SaveSettings);
            // Initialize Commands
            CaptureStandardImageCommand = new RelayCommand(CaptureStandardImage);
            SaveCapturedImageCommand = new RelayCommand(SaveCapturedImage);
            DeleteCapturedImageCommand = new RelayCommand(DeleteCapturedImage);


        }

        private ApplicationSettings LoadSettings()
        {
            var jsonService = new JsonDatabaseService<ApplicationSettings>("settings.json");
            return jsonService.LoadData() ?? new ApplicationSettings();  // Return default if no saved settings found
        }

        // Handles changes in the camera collection.
        private void Cameras_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (CameraModel cam in e.NewItems)
                {
                    if (cam is INotifyPropertyChanged notifier)
                    {
                        notifier.PropertyChanged += (s, ev) =>
                        {
                            //if (ApplicationSettings.Cameras.Count > 0 && ApplicationSettings.Cameras[0] == cam)
                                //CheckCameraConnection(null);
                        };
                    }
                }
            }
        }

        // Adds a new camera to the list.
        private void AddCamera(object parameter)
        {
            var newCamera = new CameraModel
            {
                CameraIndex = ApplicationSettings.Cameras.Count,
                Description = "New Camera"
            };
            ApplicationSettings.Cameras.Add(newCamera);
            //if (ApplicationSettings.Cameras.Count == 1)
            //    CheckCameraConnection(null);
        }

        // Registers new PLC endpoints.
        private void RegisterPLCEndpoints(object parameter)
        {
            ApplicationSettings.PLCEndpoints.Add(new PLCEndpoint
            {
                Name = "Endpoint " + (ApplicationSettings.PLCEndpoints.Count + 1),
                RegisterAddress = 100
            });
        }

        // Adds a preprocessing step.
        private void AddPreprocessingStep(object parameter)
        {
            int order = ApplicationSettings.PreprocessingSteps.Count + 1;
            ApplicationSettings.PreprocessingSteps.Add(new PreprocessingStep
            {
                Order = order,
                StepName = "New Step",
                Parameters = ""
            });
        }

        // Removes the last preprocessing step.
        private void RemovePreprocessingStep(object parameter)
        {
            if (ApplicationSettings.PreprocessingSteps.Count > 0)
            {
                ApplicationSettings.PreprocessingSteps.RemoveAt(ApplicationSettings.PreprocessingSteps.Count - 1);
            }
        }

        // Sorts preprocessing steps by their order.
        private void SortPreprocessingSteps(object parameter)
        {
            var sorted = new System.Collections.Generic.List<PreprocessingStep>(ApplicationSettings.PreprocessingSteps);
            sorted.Sort((x, y) => x.Order.CompareTo(y.Order));
            ApplicationSettings.PreprocessingSteps.Clear();
            foreach (var step in sorted)
            {
                ApplicationSettings.PreprocessingSteps.Add(step);
            }
        }

        // Checks the connectivity of the first configured camera.
        // Kiểm tra kết nối camera bất đồng bộ
        private async void CheckCameraConnection(object parameter)
        {
            foreach (var camera in ApplicationSettings.Cameras)
            {
                try
                {
                    using (var cameraService = new CameraService(camera.CameraIndex))
                    {
                        var frame = await Task.Run(() => cameraService.GetCurrentFrame());
                        // Cập nhật UI qua Dispatcher
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            camera.IsConnected = (frame != null);
                            camera.ConnectionColor = (frame != null) ? "Green" : "Gray";
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi camera {camera.CameraIndex}: {ex.Message}");
                }
            }
        }


        // Checks the connectivity of the PLC by attempting to read a register.
        private void CheckPLCConnection(object parameter)
        {
            try
            {
                using (var modbusService = new ModbusService(ApplicationSettings.PLCConfiguration))
                {
                    var registers = modbusService.ReadHoldingRegisters(0, 1);
                    PLCConnected = (registers != null);
                }
            }
            catch (Exception ex)
            {
                PLCConnected = false;
                Console.WriteLine($"PLC connection error: {ex.Message}");
            }
        }

        // Saves the current application settings to a JSON file.
        private void SaveSettings(object parameter)
        {
            try
            {
                var jsonService = new JsonDatabaseService<ApplicationSettings>("settings.json");
                jsonService.SaveData(ApplicationSettings);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}");
            }
        }
        private async void CaptureStandardImage(object parameter)
        {
            // Get the selected camera from the parameter
            if (parameter is CameraModel selectedCamera)
            {
                if (string.IsNullOrEmpty(ApplicationSettings.ImageStoragePath) || !Directory.Exists(ApplicationSettings.ImageStoragePath))
                {
                    MessageBox.Show("Invalid Image Storage Path");
                    return;
                }

                string directoryPath = Path.Combine(ApplicationSettings.ImageStoragePath, "Standard");
                Directory.CreateDirectory(directoryPath); // Ensure the directory exists

                try
                {
                    // Capture image from the selected camera
                    using (var cameraService = new CameraService(selectedCamera.CameraIndex))  // Use CameraIndex from selected camera
                    {
                        var frame = await Task.Run(() => cameraService.GetCurrentFrame());
                        if (frame != null)
                        {
                            // Convert captured frame to BitmapSource
                            CapturedImage = frame.ToBitmapSource();

                            // Save the captured image
                            string imagePath = Path.Combine(directoryPath, "StandardImage.jpg");
                            frame.Save(imagePath);  // Save the image to disk

                            // Update the StandardImagePath for the selected camera
                            selectedCamera.StandardImagePath = imagePath;
                            MessageBox.Show("Image captured and saved successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Failed to capture image.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error capturing image: {ex.Message}");
                }
            }
        }

        private void SaveCapturedImage(object parameter)
        {
            if (parameter is CameraModel selectedCamera && !string.IsNullOrEmpty(selectedCamera.StandardImagePath))
            {
                try
                {
                    // Update the StandardImagePath for the selected camera
                    selectedCamera.StandardImagePath = selectedCamera.StandardImagePath;
                    MessageBox.Show("Image path updated successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving image path: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("No image captured or path not set.");
            }
        }

        private void DeleteCapturedImage(object parameter)
        {
            if (parameter is CameraModel selectedCamera && !string.IsNullOrEmpty(selectedCamera.StandardImagePath))
            {
                try
                {
                    if (File.Exists(selectedCamera.StandardImagePath))
                    {
                        File.Delete(selectedCamera.StandardImagePath);
                        selectedCamera.StandardImagePath = string.Empty; // Clear the path after deletion
                        MessageBox.Show("Image deleted.");
                        CapturedImage = null; // Clear the displayed image
                    }
                    else
                    {
                        MessageBox.Show("No image to delete.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting image: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("No image captured.");
            }
        }
        private CameraModel _selectedCamera;
        public CameraModel SelectedCamera
        {
            get => _selectedCamera;
            set
            {
                _selectedCamera = value;
                OnPropertyChanged();
            }
        }

        // When loading settings or saving updates, make sure the StandardImagePath is properly updated.
        public void UpdateStandardImagePath(string path)
        {
            SelectedCamera.StandardImagePath = path;
            var jsonService = new JsonDatabaseService<ApplicationSettings>("settings.json");
            jsonService.SaveData(ApplicationSettings);  // Save the updated settings
        }


    }
}
