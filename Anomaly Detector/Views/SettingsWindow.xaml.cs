using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
            if (sender is GroupBox groupBox && groupBox.DataContext is CameraModel camera)
            {
                // Mở CameraWindow và truyền thông tin của camera đã được chọn
                CameraWindow cameraWindow = new CameraWindow(camera);
                cameraWindow.ShowDialog(); // Mở cửa sổ CameraWindow
            }
        }

    }
}
