using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using Api.BusinessLogicLayer.Interfaces;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

namespace Api.BusinessLogicLayer.Models
{
    public class PushNotification : IPushNotification
    {
        private IList<string> _devices = new List<string>();
    
        private IDictionary<string, string> _customData = new Dictionary<string, string>();

        public string Name { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public IList<string> Devices => _devices;
        public IDictionary<string, string> CustomData => _customData;

        public void AddDeviceId(string deviceId)
        {
            _devices.Add(deviceId);
        }

        public void AddCustomData(string key, string value)
        {
            _customData.Add(key, value);
        }
    }
}