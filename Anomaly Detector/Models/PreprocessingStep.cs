namespace Anomaly_Detector.Models
{
    /// <summary>
    /// Represents a single step in the image preprocessing pipeline.
    /// </summary>
    public class PreprocessingStep
    {
        /// <summary>
        /// Gets or sets the name of the preprocessing step.
        /// </summary>
        public string StepName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the parameters associated with this step.
        /// </summary>
        public string Parameters { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the order index for sorting steps.
        /// </summary>
        public int Order { get; set; }
    }
}
