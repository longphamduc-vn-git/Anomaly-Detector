using System.Collections.ObjectModel;

namespace Anomaly_Detector.Models
{
    /// <summary>
    /// Represents the overall application settings including camera, PLC, and image processing configurations.
    /// </summary>
    public class ApplicationSettings
    {
        /// <summary>
        /// Gets or sets the collection of camera configurations.
        /// </summary>
        public ObservableCollection<CameraModel> Cameras { get; set; } = new ObservableCollection<CameraModel>();

        /// <summary>
        /// Gets or sets the PLC configuration.
        /// </summary>
        public PLCModel PLCConfiguration { get; set; } = new PLCModel();

        /// <summary>
        /// Gets or sets the collection of registered PLC endpoints.
        /// </summary>
        public ObservableCollection<PLCEndpoint> PLCEndpoints { get; set; } = new ObservableCollection<PLCEndpoint>();

        /// <summary>
        /// Gets or sets the settings for image preprocessing.
        /// </summary>
        public ImagePreprocessingSettings PreprocessingSettings { get; set; } = new ImagePreprocessingSettings();

        /// <summary>
        /// Gets or sets the folder path where images are stored.
        /// </summary>
        public string ImageStoragePath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the threshold value for image comparison or anomaly detection.
        /// </summary>
        public double Threshold { get; set; }

        /// <summary>
        /// Gets or sets the ordered collection of image preprocessing steps.
        /// </summary>
        public ObservableCollection<PreprocessingStep> PreprocessingSteps { get; set; } = new ObservableCollection<PreprocessingStep>();
    }
}
