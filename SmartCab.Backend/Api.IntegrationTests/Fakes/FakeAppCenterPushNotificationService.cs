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

        public FakeAppCenterPushNotificationService()
        {
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
