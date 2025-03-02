using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;

namespace Anomaly_Detector.Services
{
    /// <summary>
    /// Provides methods for comparing two images and highlighting their differences.
    /// </summary>
    public static class ImageComparison
    {
        /// <summary>
        /// Computes the absolute difference between two images.
        /// The images must be in Bgr format and have the same dimensions.
        /// </summary>
        /// <param name="image1">The first input image in Bgr format.</param>
        /// <param name="image2">The second input image in Bgr format.</param>
        /// <returns>An image representing the absolute difference between the two input images.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either image is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the images have different sizes.</exception>
        public static Image<Bgr, byte> ComputeAbsoluteDifference(Image<Bgr, byte> image1, Image<Bgr, byte> image2)
        {
            if (image1 == null)
                throw new ArgumentNullException(nameof(image1));
            if (image2 == null)
                throw new ArgumentNullException(nameof(image2));
            if (image1.Size != image2.Size)
                throw new ArgumentException("Images must be of the same size.");

            return image1.AbsDiff(image2);
        }

        /// <summary>
        /// Computes the mean squared error (MSE) between two images.
        /// The images must be in Bgr format and have the same dimensions.
        /// </summary>
        /// <param name="image1">The first input image in Bgr format.</param>
        /// <param name="image2">The second input image in Bgr format.</param>
        /// <returns>The mean squared error as a double value.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either image is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the images have different sizes.</exception>
        public static double ComputeMeanSquaredError(Image<Bgr, byte> image1, Image<Bgr, byte> image2)
        {
            if (image1 == null)
                throw new ArgumentNullException(nameof(image1));
            if (image2 == null)
                throw new ArgumentNullException(nameof(image2));
            if (image1.Size != image2.Size)
                throw new ArgumentException("Images must be of the same size.");

            // Get the absolute difference image and convert it to grayscale.
            var diffImage = image1.AbsDiff(image2);
            var grayDiff = diffImage.Convert<Gray, byte>();

            double sumSquared = 0;
            int totalPixels = grayDiff.Width * grayDiff.Height;

            // Iterate over each pixel to accumulate the squared differences.
            for (int y = 0; y < grayDiff.Height; y++)
            {
                for (int x = 0; x < grayDiff.Width; x++)
                {
                    byte pixelValue = grayDiff.Data[y, x, 0];
                    sumSquared += pixelValue * pixelValue;
                }
            }
            return sumSquared / totalPixels;
        }

        /// <summary>
        /// Highlights differences between two images by overlaying red on regions where the absolute difference exceeds a threshold.
        /// </summary>
        /// <param name="image1">The first input image in Bgr format (this image will be used as the base).</param>
        /// <param name="image2">The second input image in Bgr format.</param>
        /// <param name="threshold">A threshold value for determining significant differences (default is 30).</param>
        /// <returns>An image with significant differences highlighted in red.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either image is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the images have different sizes.</exception>
        public static Image<Bgr, byte> HighlightDifferences(Image<Bgr, byte> image1, Image<Bgr, byte> image2, double threshold = 30)
        {
            // Validate input images
            if (image1 == null)
                throw new ArgumentNullException(nameof(image1), "Input image 1 cannot be null.");
            if (image2 == null)
                throw new ArgumentNullException(nameof(image2), "Input image 2 cannot be null.");
            if (image1.Size != image2.Size)
                throw new ArgumentException("Images must be of the same size.");

            try
            {
                // Step 1: Compute the absolute difference between the two images
                var diffImage = image1.AbsDiff(image2);

                // Step 2: Convert the difference image to grayscale
                var grayDiff = diffImage.Convert<Gray, byte>();

                // Step 3: Create a binary mask where differences exceed the threshold
                var mask = new Image<Gray, byte>(grayDiff.Size);
                CvInvoke.Threshold(grayDiff, mask, threshold, 255, ThresholdType.Binary);

                // Step 4: Create a copy of the original image to highlight differences
                var highlightedImage = image1.Copy();

                // Step 5: Create a red mask for highlighting
                var redMask = new Image<Bgr, byte>(highlightedImage.Size);
                redMask.SetValue(new Bgr(0, 0, 255)); // Red color (B=0, G=0, R=255)

                // Step 6: Apply the red mask to the highlighted image where differences are significant
                CvInvoke.BitwiseAnd(redMask, redMask, highlightedImage, mask);

                return highlightedImage;
            }
            catch (Exception ex)
            {
                // Log the error and rethrow if necessary
                Console.WriteLine($"Error highlighting differences: {ex.Message}");
                throw;
            }
        }
    }
}
