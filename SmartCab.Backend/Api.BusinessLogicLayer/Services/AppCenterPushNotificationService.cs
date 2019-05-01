using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
using Newtonsoft.Json;

namespace Api.BusinessLogicLayer.Services
{
    public class AppCenterPushNotificationService : IPushNotificationService
    {
        private HttpClient _httpClient;

        private const string BaseUrl = "https://api.appcenter.ms/v0.1/apps/";
        private const string Organization = "frank.andersen-gmail.com";
        private const string Android = "SmartCab";
        private const string PushNotificationUri = "push/notifications";

        private const string ApiKeyName = "X-API-Token";
        private const string ApiKey = "7c415e6a7cb8feab721420bd9038c123625e3bfc"; // This Token is named "Backend" in AppCenter

        public AppCenterPushNotificationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendAsync(IPushNotification notification)
        {
            var url = new Uri($"{BaseUrl}/{Organization}/{Android}/{PushNotificationUri}");
            var content = new StringContent(CreateJson(notification), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add(ApiKeyName, ApiKey);
            await _httpClient.PostAsync(url, content);
        }

        private string CreateJson(IPushNotification notification)
        {
            return JsonConvert.SerializeObject(new
            {
                notification_target = new
                {
                    type = "devices_target",
                    devices = notification.Devices.ToArray()
                },
                notification_content = new
                {
                    name = notification.Name,
                    title = notification.Title,
                    body = notification.Body,
                    custom_data = notification.CustomData
                }
            });
        }
    }
}