using System;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

namespace Anomaly_Detector.Services
{
    /// <summary>
    /// Provides methods for saving and loading data in JSON format.
    /// </summary>
    /// <typeparam name="T">The type of data to be serialized and deserialized.</typeparam>
    public class JsonDatabaseService<T> where T : class
    {
        private readonly string _filePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDatabaseService{T}"/> class.
        /// </summary>
        /// <param name="filePath">The file path where the JSON data will be stored.</param>
        public JsonDatabaseService(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Saves the specified data to a JSON file.
        /// </summary>
        /// <param name="data">The data to be saved.</param>
        public void SaveData(T data)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(_filePath, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving JSON data: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads data from a JSON file.
        /// </summary>
        /// <returns>
        /// The deserialized data, or null if the file does not exist or an error occurs.
        /// </returns>
        public T? LoadData()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string jsonData = File.ReadAllText(_filePath);
                    return JsonConvert.DeserializeObject<T>(jsonData);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading JSON data: {ex.Message}");
                return null;
            }
        }
    }
}
