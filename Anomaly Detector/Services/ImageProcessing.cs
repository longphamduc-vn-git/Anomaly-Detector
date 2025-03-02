using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;

namespace Anomaly_Detector.Services
{
    /// <summary>
    /// Provides various image preprocessing functionalities such as converting to grayscale,
    /// noise filtering, light balancing, contrast enhancement, resizing, and geometric transformations.
    /// </summary>
    public static class ImagePreprocessing
    {
        /// <summary>
        /// Converts a BGR image to a grayscale image.
        /// </summary>
        /// <param name="inputImage">The input image in Bgr format.</param>
        /// <returns>An image converted to grayscale.</returns>
        /// <exception cref="ArgumentNullException">Thrown if inputImage is null.</exception>
        public static Image<Gray, byte> ConvertToGrayscale(Image<Bgr, byte> inputImage)
        {
            if (inputImage == null)
                throw new ArgumentNullException(nameof(inputImage), "Input image cannot be null.");
            return inputImage.Convert<Gray, byte>();
        }

        /// <summary>
        /// Resizes the given BGR image to the specified width and height.
        /// </summary>
        /// <param name="inputImage">The input image in Bgr format.</param>
        /// <param name="width">The desired width.</param>
        /// <param name="height">The desired height.</param>
        /// <returns>The resized image.</returns>
        /// <exception cref="ArgumentNullException">Thrown if inputImage is null.</exception>
        public static Image<Bgr, byte> ResizeImage(Image<Bgr, byte> inputImage, int width, int height)
        {
            if (inputImage == null)
                throw new ArgumentNullException(nameof(inputImage), "Input image cannot be null.");
            return inputImage.Resize(width, height, Inter.Linear);
        }

        /// <summary>
        /// Applies a Gaussian blur to the input BGR image.
        /// </summary>
        /// <param name="inputImage">The input image in Bgr format.</param>
        /// <param name="kernelSize">The size of the Gaussian kernel (must be an odd number, e.g., 3, 5, 7).</param>
        /// <returns>The blurred image.</returns>
        /// <exception cref="ArgumentNullException">Thrown if inputImage is null.</exception>
        /// <exception cref="ArgumentException">Thrown if kernelSize is not an odd number.</exception>
        public static Image<Bgr, byte> ApplyGaussianBlur(Image<Bgr, byte> inputImage, int kernelSize = 5)
        {
            if (inputImage == null)
                throw new ArgumentNullException(nameof(inputImage), "Input image cannot be null.");
            if (kernelSize % 2 == 0)
                throw new ArgumentException("Kernel size must be an odd number.", nameof(kernelSize));
            return inputImage.SmoothGaussian(kernelSize);
        }

        /// <summary>
        /// Applies a binary threshold to a grayscale image.
        /// </summary>
        /// <param name="inputImage">The input image in grayscale.</param>
        /// <param name="thresholdValue">The threshold value.</param>
        /// <returns>The binary image resulting from thresholding.</returns>
        /// <exception cref="ArgumentNullException">Thrown if inputImage is null.</exception>
        public static Image<Gray, byte> ApplyThreshold(Image<Gray, byte> inputImage, double thresholdValue)
        {
            if (inputImage == null)
                throw new ArgumentNullException(nameof(inputImage), "Input image cannot be null.");
            Image<Gray, byte> binaryImage = inputImage.CopyBlank();
            CvInvoke.Threshold(inputImage, binaryImage, thresholdValue, 255, ThresholdType.Binary);
            return binaryImage;
        }

        /// <summary>
        /// Applies a median filter to reduce noise in a BGR image.
        /// </summary>
        /// <param name="inputImage">The input image in Bgr format.</param>
        /// <param name="kernelSize">The size of the median filter kernel (must be an odd number, e.g., 3, 5, 7).</param>
        /// <returns>The denoised image.</returns>
        /// <exception cref="ArgumentNullException">Thrown if inputImage is null.</exception>
        /// <exception cref="ArgumentException">Thrown if kernelSize is not an odd number.</exception>
        public static Image<Bgr, byte> ApplyMedianFilter(Image<Bgr, byte> inputImage, int kernelSize = 3)
        {
            if (inputImage == null)
                throw new ArgumentNullException(nameof(inputImage), "Input image cannot be null.");
            if (kernelSize % 2 == 0)
                throw new ArgumentException("Kernel size must be an odd number.", nameof(kernelSize));
            Image<Bgr, byte> filteredImage = inputImage.CopyBlank();
            CvInvoke.MedianBlur(inputImage, filteredImage, kernelSize);
            return filteredImage;
        }

        /// <summary>
        /// Applies histogram equalization to a grayscale image to balance lighting.
        /// </summary>
        /// <param name="inputImage">The input grayscale image.</param>
        /// <returns>The image after histogram equalization.</returns>
        /// <exception cref="ArgumentNullException">Thrown if inputImage is null.</exception>
        public static Image<Gray, byte> EqualizeHistogram(Image<Gray, byte> inputImage)
        {
            if (inputImage == null)
                throw new ArgumentNullException(nameof(inputImage), "Input image cannot be null.");
            Image<Gray, byte> equalizedImage = inputImage.CopyBlank();
            CvInvoke.EqualizeHist(inputImage, equalizedImage);
            return equalizedImage;
        }

        // Replace the EnhanceContrastCLAHE method with the following code
        /// <summary>
        /// Enhances the contrast of a grayscale image using CLAHE (Contrast Limited Adaptive Histogram Equalization).
        /// </summary>
        /// <param name="inputImage">The input grayscale image.</param>
        /// <param name="clipLimit">The threshold for contrast limiting (default is 2.0).</param>
        /// <param name="tileGridSize">The size of the grid for the histogram equalization (default is 8x8).</param>
        /// <returns>The contrast-enhanced image.</returns>
        /// <exception cref="ArgumentNullException">Thrown if inputImage is null.</exception>
        //public static Image<Gray, byte> EnhanceContrastCLAHE(Image<Gray, byte> inputImage, double clipLimit = 2.0, Size tileGridSize = default)
        //{
        //    if (inputImage == null)
        //        throw new ArgumentNullException(nameof(inputImage), "Input image cannot be null.");

        //    if (tileGridSize == default)
        //        tileGridSize = new Size(8, 8);

        //    using (var clahe = new CudaCLAHE(clipLimit, tileGridSize))
        //    {
        //        Image<Gray, byte> result = inputImage.CopyBlank();
        //        clahe.Apply(inputImage, result);
        //        return result;
        //    }
        //}

        /// <summary>
        /// Adjusts the brightness of a BGR image by adding a constant value to each pixel.
        /// </summary>
        /// <param name="inputImage">The input image in Bgr format.</param>
        /// <param name="brightness">The brightness adjustment value (positive to increase brightness, negative to decrease).</param>
        /// <returns>The brightness-adjusted image.</returns>
        /// <exception cref="ArgumentNullException">Thrown if inputImage is null.</exception>
        public static Image<Bgr, byte> AdjustBrightness(Image<Bgr, byte> inputImage, int brightness)
        {
            if (inputImage == null)
                throw new ArgumentNullException(nameof(inputImage), "Input image cannot be null.");

            // Create a new image and add brightness using AddWeighted (alpha=1.0, beta=0.0, gamma=brightness)
            Image<Bgr, byte> adjustedImage = inputImage.Copy();
            CvInvoke.AddWeighted(inputImage, 1.0, adjustedImage, 0.0, brightness, adjustedImage);
            return adjustedImage;
        }

        /// <summary>
        /// Rotates a BGR image by a specified angle.
        /// </summary>
        /// <param name="inputImage">The input image in Bgr format.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <returns>The rotated image.</returns>
        /// <exception cref="ArgumentNullException">Thrown if inputImage is null.</exception>
        public static Image<Bgr, byte> RotateImage(Image<Bgr, byte> inputImage, double angle)
        {
            if (inputImage == null)
                throw new ArgumentNullException(nameof(inputImage), "Input image cannot be null.");

            PointF center = new PointF(inputImage.Width / 2f, inputImage.Height / 2f);
            using (var rotationMatrix = new Emgu.CV.Matrix<double>(2, 3))
            {
                CvInvoke.GetRotationMatrix2D(center, angle, 1.0, rotationMatrix);
                Image<Bgr, byte> rotated = inputImage.CopyBlank();
                CvInvoke.WarpAffine(inputImage, rotated, rotationMatrix, inputImage.Size, Inter.Linear, Warp.Default, BorderType.Constant, new MCvScalar());
                return rotated;
            }
        }

        /// <summary>
        /// Applies an affine transformation to a BGR image using the specified transformation matrix.
        /// </summary>
        /// <param name="inputImage">The input image in Bgr format.</param>
        /// <param name="affineMatrix">A 2x3 affine transformation matrix.</param>
        /// <returns>The transformed image.</returns>
        /// <exception cref="ArgumentNullException">Thrown if inputImage or affineMatrix is null.</exception>
        public static Image<Bgr, byte> ApplyAffineTransformation(Image<Bgr, byte> inputImage, Emgu.CV.Matrix<double> affineMatrix)
        {
            if (inputImage == null)
                throw new ArgumentNullException(nameof(inputImage), "Input image cannot be null.");
            if (affineMatrix == null)
                throw new ArgumentNullException(nameof(affineMatrix), "Affine transformation matrix cannot be null.");

            Image<Bgr, byte> transformed = inputImage.CopyBlank();
            CvInvoke.WarpAffine(inputImage, transformed, affineMatrix, inputImage.Size, Inter.Linear, Warp.Default, BorderType.Constant, new MCvScalar());
            return transformed;
        }
    }
}
