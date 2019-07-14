using System.Collections.Generic;
using Api.BusinessLogicLayer.Interfaces;

namespace Api.BusinessLogicLayer.Models
{
    /// <summary>
    /// Data object model for containing details of a push notification
    /// </summary>
    public class PushNotification : IPushNotification
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public IList<string> Devices { get; } = new List<string>();
        public IDictionary<string, string> CustomData { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Adds a recipient to the notification
        /// </summary>
        /// <param name="deviceId">A device identifier</param>
        public void AddDeviceId(string deviceId)
        {
            Devices.Add(deviceId);
        }

        /// <summary>
        /// Adds a key-value pair of data to the notification
        /// </summary>
        /// <param name="key">Identifier for later retrieval of the given value</param>
        /// <param name="value">Value to be embedded in the notification</param>
        public void AddCustomData(string key, string value)
        {
            CustomData.Add(key, value);
        }
    }
}