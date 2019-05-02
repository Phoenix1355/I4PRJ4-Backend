using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Models;

namespace Api.BusinessLogicLayer.Factories
{
    /// <summary>
    /// Factory class for instantiating new PushNotifications.
    /// </summary>
    public class PushNotificationFactory : IPushNotificationFactory
    {
        /// <summary>
        /// Instantiates and returns a new instance of the PushNotification class.
        /// </summary>
        /// <returns>A new instance of PushNotification class.</returns>
        public IPushNotification GetPushNotification()
        {
            return new PushNotification();
        }
    }
}