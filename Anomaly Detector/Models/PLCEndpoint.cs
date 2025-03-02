namespace Anomaly_Detector.Models
{
    /// <summary>
    /// Represents a PLC endpoint registered in the application.
    /// </summary>
    public class PLCEndpoint
    {
        /// <summary>
        /// Gets or sets the name or identifier of the endpoint.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the register address for the endpoint.
        /// </summary>
        public ushort RegisterAddress { get; set; }
    }
}
