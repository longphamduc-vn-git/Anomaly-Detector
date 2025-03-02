using System;
using System.Net.Sockets;
using Modbus.Device;  // Requires NModbus4 NuGet package
using Anomaly_Detector.Models;

namespace Anomaly_Detector.Services
{
    /// <summary>
    /// Provides methods for connecting to and communicating with a PLC using Modbus TCP.
    /// </summary>
    public class ModbusService : IDisposable
    {
        // Mark fields as nullable to resolve non-nullable warnings.
        private TcpClient? _tcpClient;
        private IModbusMaster? _modbusMaster;
        private readonly PLCModel _plcModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModbusService"/> class with the specified PLC configuration.
        /// </summary>
        /// <param name="plcModel">The PLC configuration model.</param>
        public ModbusService(PLCModel plcModel)
        {
            _plcModel = plcModel;
            Connect();
        }

        /// <summary>
        /// Establishes a connection to the PLC using the provided configuration.
        /// </summary>
        private void Connect()
        {
            try
            {
                _tcpClient = new TcpClient(_plcModel.IPAddress, _plcModel.Port);
                // Create a Modbus master instance using the TCP connection.
                _modbusMaster = ModbusIpMaster.CreateIp(_tcpClient);
                Console.WriteLine("Connected to PLC successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to PLC: {ex.Message}");
                // Additional logging or error handling can be added here.
            }
        }

        /// <summary>
        /// Reads a range of holding registers from the PLC.
        /// </summary>
        /// <param name="startAddress">The starting register address.</param>
        /// <param name="numberOfPoints">The number of registers to read.</param>
        /// <returns>An array of unsigned shorts containing the register values, or null if an error occurs.</returns>
        public ushort[]? ReadHoldingRegisters(ushort startAddress, ushort numberOfPoints)
        {
            if (_modbusMaster == null)
            {
                Console.WriteLine("Modbus master is not initialized.");
                return null;
            }

            try
            {
                // Cast the UnitID from int to byte
                return _modbusMaster.ReadHoldingRegisters((byte)_plcModel.UnitID, startAddress, numberOfPoints);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading registers: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Writes a single value to a specific register on the PLC.
        /// </summary>
        /// <param name="registerAddress">The register address to write to.</param>
        /// <param name="value">The value to write.</param>
        public void WriteSingleRegister(ushort registerAddress, ushort value)
        {
            if (_modbusMaster == null)
            {
                Console.WriteLine("Modbus master is not initialized.");
                return;
            }

            try
            {
                // Cast UnitID from int to byte
                _modbusMaster.WriteSingleRegister((byte)_plcModel.UnitID, registerAddress, value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to register: {ex.Message}");
            }
        }

        /// <summary>
        /// Disconnects from the PLC and releases the TCP connection.
        /// </summary>
        public void Disconnect()
        {
            if (_tcpClient != null)
            {
                _tcpClient.Close();
                _tcpClient = null;
            }
            Console.WriteLine("PLC connection disconnected.");
        }

        /// <summary>
        /// Releases all resources used by the <see cref="ModbusService"/>.
        /// </summary>
        public void Dispose()
        {
            Disconnect();
            _modbusMaster?.Dispose();
        }
    }
}
