namespace Anomaly_Detector.Models
{
    /// <summary>
    /// Represents the configuration model for a PLC connection.
    /// </summary>
    public class PLCModel
    {
        /// <summary>
        /// Gets or sets the IP address of the PLC.
        /// </summary>
        public string IPAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the port number to connect (usually 502 for Modbus TCP).
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the Unit ID of the PLC on the Modbus network.
        /// </summary>
        public int UnitID { get; set; }

        /// <summary>
        /// Gets or sets the description for the PLC.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
