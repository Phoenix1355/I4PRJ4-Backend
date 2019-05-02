using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using Api.BusinessLogicLayer.Interfaces;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

namespace Api.BusinessLogicLayer.Models
{
    public class PushNotification : IPushNotification
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public IList<string> Devices { get; } = new List<string>();
        public IDictionary<string, string> CustomData { get; } = new Dictionary<string, string>();

        public void AddDeviceId(string deviceId)
        {
            Devices.Add(deviceId);
        }

        public void AddCustomData(string key, string value)
        {
            CustomData.Add(key, value);
        }
    }
}