using System.Threading.Tasks;

namespace Api.BusinessLogicLayer.Interfaces
{
    public interface IPushNotificationService
    {
        Task SendAsync(IPushNotification notification);
    }
}