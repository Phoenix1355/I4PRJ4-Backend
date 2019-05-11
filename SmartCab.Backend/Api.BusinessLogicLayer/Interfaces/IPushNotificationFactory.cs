namespace Api.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Factory of push notification.
    /// </summary>
    public interface IPushNotificationFactory
    {
        IPushNotification GetPushNotification();
    }
}