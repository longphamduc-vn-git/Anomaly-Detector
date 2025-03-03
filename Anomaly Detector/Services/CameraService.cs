using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Anomaly_Detector.Services
{
    public class CameraService : IDisposable
    {
        private VideoCapture _capture;

        public CameraService(int cameraIndex = 0)
        {
            _capture = new VideoCapture(cameraIndex);
            if (!_capture.IsOpened)
            {
                throw new Exception("Unable to open the camera.");
            }
        }

        public Image<Bgr, byte>? GetCurrentFrame()
        {
            var frameMat = _capture?.QueryFrame();
            return frameMat?.ToImage<Bgr, byte>();
        }

        public void SaveImage(BitmapImage image, string filePath)
        {
            image.ToMat().ToImage<Bgr, byte>().Save(filePath);
        }

        public void Dispose()
        {
            _capture?.Dispose();
        }
    }
}
