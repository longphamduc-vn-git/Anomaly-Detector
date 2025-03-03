using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Anomaly_Detector.Models;

namespace Anomaly_Detector.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void CameraItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedCamera = (CameraModel)((DataGrid)sender).SelectedItem;
            if (selectedCamera != null)
            {
                // Open the camera screen (you can customize this to open the camera view)
                var cameraWindow = new CameraWindow(selectedCamera);  // Assuming you have a CameraWindow
                cameraWindow.Show();
            }
        }
    }
}
