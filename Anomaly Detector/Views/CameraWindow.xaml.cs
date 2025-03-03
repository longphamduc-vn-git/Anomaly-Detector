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
using Anomaly_Detector.ViewModels;

namespace Anomaly_Detector.Views
{
    /// <summary>
    /// Interaction logic for CameraWindow.xaml
    /// </summary>
    public partial class CameraWindow : Window
    {
        public CameraWindow(CameraModel camera)
        {
            InitializeComponent();

            // Access the ViewModel and initialize the camera
            var viewModel = (CameraWindowViewModel)DataContext;
            viewModel.InitializeCamera(camera); // Set the camera model

        }
    }

}
