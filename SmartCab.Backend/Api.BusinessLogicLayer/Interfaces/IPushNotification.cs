using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Api.BusinessLogicLayer.Interfaces
{
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