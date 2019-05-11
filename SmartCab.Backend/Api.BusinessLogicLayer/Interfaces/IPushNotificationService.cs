using System.Threading.Tasks;

namespace Api.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Interface for services that sends notifications to phones.
    /// </summary>
    public interface IPushNotificationService
    {
        Task SendAsync(IPushNotification notification);
    }
}