using System;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Anomaly_Detector.Services
{
    /// <summary>
    /// Provides methods to manage a camera connection and retrieve frames using Emgu.CV.
    /// </summary>
    public class CameraService : IDisposable
    {
        private VideoCapture? _capture;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraService"/> class.
        /// Connects to the specified camera by index (default is 0).
        /// </summary>
        /// <param name="cameraIndex">The index of the camera to connect to. Default is 0.</param>
        public CameraService(int cameraIndex = 0)
        {
            try
            {
                _capture = new VideoCapture(cameraIndex);
                if (!_capture.IsOpened)
                {
                    throw new Exception("Unable to open the camera.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing camera: {ex.Message}");
            }
        }

        /// <summary>
        /// Captures and returns the current frame from the camera.
        /// </summary>
        /// <returns>
        /// An <see cref="Image{Bgr, byte}"/> representing the current frame,
        /// or <c>null</c> if the frame could not be captured.
        /// </returns>
        public Image<Bgr, byte>? GetCurrentFrame()
        {
            if (_capture == null)
            {
                Console.WriteLine("Camera is not initialized.");
                return null;
            }

            try
            {
                var frameMat = _capture.QueryFrame();
                if (frameMat != null)
                {
                    return frameMat.ToImage<Bgr, byte>();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error capturing frame: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Releases the camera resources.
        /// </summary>
        public void Release()
        {
            if (_capture != null)
            {
                _capture.Dispose();
                _capture = null;
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="CameraService"/>.
        /// </summary>
        public void Dispose()
        {
            Release();
        }
    }
}
