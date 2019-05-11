using System.Collections.Generic;

namespace Api.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Interface for push notification. 
    /// </summary>
    public interface IPushNotification
    {
        string Name { get; set; }

        string Title { get; set; }

        string Body { get; set; }

        IList<string> Devices { get; }

        IDictionary<string, string> CustomData { get; }

        void AddDeviceId(string deviceId);

        void AddCustomData(string key, string value);
    }
}