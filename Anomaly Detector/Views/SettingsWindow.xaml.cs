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

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Get the selected camera item from the DataGrid
            var selectedCamera = (CameraModel)((DataGrid)sender).SelectedItem;

            if (selectedCamera != null)
            {
                // Open Camera Window and pass the selected camera model
                CameraWindow cameraWindow = new CameraWindow(selectedCamera);
                cameraWindow.Show();
            }
        }

    }
}
