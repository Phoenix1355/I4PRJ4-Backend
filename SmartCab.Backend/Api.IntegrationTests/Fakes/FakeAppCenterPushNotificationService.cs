using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;

namespace Api.IntegrationTests.Fakes
{
    class FakeAppCenterPushNotificationService : IPushNotificationService
    {
        private HttpClient _httpClient;


        public FakeAppCenterPushNotificationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
