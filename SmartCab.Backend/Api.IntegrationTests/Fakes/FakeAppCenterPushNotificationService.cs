using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Api.IntegrationTests.Fakes
{
    class FakeAppCenterPushNotificationService : IPushNotificationService
    {
        private HttpClient _httpClient;
        private const string BaseUrl = "https://api.appcenter.ms/v0.1/apps/";
        private const string Organization = "frank.andersen-gmail.com";
        private const string Android = "SmartCab";
        private const string PushNotificationUri = "push/notifications";

        private const string ApiKeyName = "X-API-Token";
        private readonly string _apiKey; // This Token is named "Backend" in AppCenter

        public FakeAppCenterPushNotificationService(IConfiguration config, HttpClient httpClient)
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
            
        }
    }
}
