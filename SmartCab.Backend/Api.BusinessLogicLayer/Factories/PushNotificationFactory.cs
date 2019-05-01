using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Models;

namespace Api.BusinessLogicLayer.Factories
{
    public class PushNotificationFactory : IPushNotificationFactory
    {
        public IPushNotification GetPushNotification()
        {
            return new PushNotification();
        }
    }
}