using Anomaly_Detector.Models;
using Anomaly_Detector.Services;
using Anomaly_Detector.ViewModels;  // Ensure this namespace is included
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows;
using Emgu.CV;
using System.Collections.ObjectModel;  // Added to make sure ObservableCollection is used

namespace Anomaly_Detector.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        #region Properties

        // Property to store application settings (e.g., cameras, PLC endpoints, preprocessing steps)
        public ApplicationSettings ApplicationSettings { get; set; }

        // Connectivity status properties for camera and PLC

        private bool _plcConnected;
        public bool PLCConnected
        {
            get => _plcConnected;
            set { _plcConnected = value; OnPropertyChanged(); }
        }

        // Property to store the captured image
        private BitmapSource? _capturedImage;
        public BitmapSource? CapturedImage
        {
            get => _capturedImage;
            set { _capturedImage = value; OnPropertyChanged(); }
        }

        // Control visibility of the capture button
        private bool _showCaptureButton;
        public bool ShowCaptureButton
        {
            get => _showCaptureButton;
            set { _showCaptureButton = value; OnPropertyChanged(); }
        }
        private CameraModel _selectedCamera;
        public CameraModel? SelectedCamera
        {
            get => _selectedCamera;
            set
            {
                _selectedCamera = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        // Commands for managing cameras, PLC endpoints, and preprocessing steps.
        public ICommand AddCameraCommand { get; set; }
        public ICommand RemoveCameraCommand { get; set; }
        public ICommand RegisterPLCEndpointsCommand { get; set; }
        public ICommand AddPreprocessingStepCommand { get; set; }
        public ICommand RemovePreprocessingStepCommand { get; set; }
        public ICommand SortPreprocessingStepsCommand { get; set; }

        // Commands for connectivity checks and image capture functionalities.
        public ICommand CheckCameraConnectionCommand { get; set; }
        public ICommand CheckPLCConnectionCommand { get; set; }
        public ICommand CaptureStandardImageCommand { get; set; }
        public ICommand SaveSettingsCommand { get; set; }
        public ICommand SaveCapturedImageCommand { get; set; }
        public ICommand DeleteCapturedImageCommand { get; set; }

        #endregion

        #region Constructor

        // Constructor: Initializes settings and commands for UI interaction
        public SettingsViewModel()
        {
            ApplicationSettings = LoadSettings(); // Load saved settings from a file
            ApplicationSettings.Cameras.CollectionChanged += Cameras_CollectionChanged;

            // Initialize commands to handle user actions
            InitializeCommands();
        }

        #endregion

        #region Methods

        // Load settings from the JSON file, or return default if none found
        private ApplicationSettings LoadSettings()
        {
            var jsonService = new JsonDatabaseService<ApplicationSettings>("settings.json");
            return jsonService.LoadData() ?? new ApplicationSettings();  // Default settings if not found
        }

        // Initialize commands that will bind to UI controls like buttons and menus
        private void InitializeCommands()
        {
            AddCameraCommand = new RelayCommand(AddCamera);
            RemoveCameraCommand = new RelayCommand(RemoveCamera);
            AddPreprocessingStepCommand = new RelayCommand(AddPreprocessingStep);
            RemovePreprocessingStepCommand = new RelayCommand(RemovePreprocessingStep);
            SortPreprocessingStepsCommand = new RelayCommand(SortPreprocessingSteps);
            CheckCameraConnectionCommand = new RelayCommand(CheckCameraConnection);
            CheckPLCConnectionCommand = new RelayCommand(CheckPLCConnection);
            SaveSettingsCommand = new RelayCommand(SaveSettings);
        }

        // Event handler for camera collection changes (e.g., adding/removing cameras)
        private void Cameras_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Update UI when the camera collection changes
            OnPropertyChanged(nameof(ApplicationSettings.Cameras)); // Notify UI about the change
        }

        #endregion

        #region Camera Management

        // Add a new camera to the collection
        private void AddCamera(object parameter)
        {
            var newCamera = new CameraModel
            {
                CameraIndex = ApplicationSettings.Cameras.Count
            };
            ApplicationSettings.Cameras.Add(newCamera);
        }

        // Remove a selected camera from the collection
        private void RemoveCamera(object parameter)
        {
            if (SelectedCamera != null)
            {
                ApplicationSettings.Cameras.Remove(SelectedCamera);
                SelectedCamera = null; // Reset selection
                OnPropertyChanged(nameof(ApplicationSettings.Cameras));
            }
            else
            {
                MessageBox.Show("Please select a camera to remove.");
            }
        }

        #endregion

        #region Camera and PLC Connectivity

        // Asynchronous method to check camera connectivity by capturing a frame
        private async void CheckCameraConnection(object parameter)
        {
            foreach (var camera in ApplicationSettings.Cameras)
            {
                try
                {
                    using (var cameraService = new CameraService(camera.CameraIndex))
                    {
                        var frame = await Task.Run(() => cameraService.GetCurrentFrame());
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            camera.IsConnected = (frame != null);
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error with camera {camera.CameraIndex}: {ex.Message}");
                }
            }
        }

        // Check PLC connection by attempting to read data from a PLC register
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

        #endregion

        
        #region Preprocessing and Settings Management

        // Add a new preprocessing step to the list
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

        // Remove the last preprocessing step from the list
        private void RemovePreprocessingStep(object parameter)
        {
            if (ApplicationSettings.PreprocessingSteps.Count > 0)
            {
                ApplicationSettings.PreprocessingSteps.RemoveAt(ApplicationSettings.PreprocessingSteps.Count - 1);
            }
        }

        // Sort preprocessing steps by their defined order
        private void SortPreprocessingSteps(object parameter)
        {
            var sorted = ApplicationSettings.PreprocessingSteps.OrderBy(step => step.Order).ToList();
            ApplicationSettings.PreprocessingSteps.Clear();
            // Add items individually to ObservableCollection
            foreach (var step in sorted)
            {
                ApplicationSettings.PreprocessingSteps.Add(step);
            }
        }

        // Save the current application settings to a JSON file
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

        #endregion
    }
}
