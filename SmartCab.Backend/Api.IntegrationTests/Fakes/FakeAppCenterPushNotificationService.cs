using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;

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
