using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Api.BusinessLogicLayer.Services
{
    /// <summary>
    /// Push notifications service for sending push notifications through AppCenter
    /// </summary>
    public class AppCenterPushNotificationService : IPushNotificationService
    {
        private HttpClient _httpClient;

        private const string BaseUrl = "https://api.appcenter.ms/v0.1/apps/";
        private const string Organization = "frank.andersen-gmail.com";
        private const string Android = "SmartCab";
        private const string PushNotificationUri = "push/notifications";

        private const string ApiKeyName = "X-API-Token";
        private readonly string _apiKey; // This Token is named "Backend" in AppCenter

        /// <summary>
        /// Constructor for the class
        /// </summary>
        /// <param name="config">The application configuration.</param>
        /// <param name="httpClient">Instance of HttpClient class.</param>
        public AppCenterPushNotificationService(IConfiguration config, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = config["AppCenterPushApiKey"];
        }

        /// <summary>
        /// Sends notification through AppCenter
        /// </summary>
        /// <param name="notification">The notification to be sent</param>
        /// <returns>Task/void</returns>
        public async Task SendAsync(IPushNotification notification)
        {
            var url = new Uri($"{BaseUrl}/{Organization}/{Android}/{PushNotificationUri}");
            var content = new StringContent(CreateJson(notification), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add(ApiKeyName, _apiKey);
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