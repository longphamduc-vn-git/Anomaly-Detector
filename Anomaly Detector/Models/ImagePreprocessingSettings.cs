namespace Anomaly_Detector.Models
{
    /// <summary>
    /// Represents settings for image preprocessing operations.
    /// </summary>
    public class ImagePreprocessingSettings
    {
        /// <summary>
        /// Gets or sets the brightness adjustment factor.
        /// </summary>
        public double Brightness { get; set; }

        /// <summary>
        /// Gets or sets the contrast adjustment factor.
        /// </summary>
        public double Contrast { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to apply noise reduction.
        /// </summary>
        public bool ApplyNoiseReduction { get; set; }

        /// <summary>
        /// Gets or sets the noise reduction method (e.g., "Gaussian", "Bilateral").
        /// </summary>
        public string NoiseReductionMethod { get; set; } = "Gaussian";
    }
}
