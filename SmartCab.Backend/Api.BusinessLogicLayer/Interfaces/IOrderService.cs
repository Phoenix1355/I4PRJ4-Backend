using System.Threading.Tasks;
using Api.BusinessLogicLayer.Responses;

namespace Api.BusinessLogicLayer.Interfaces
{
    public interface IOrderService
    {
        Task<OpenOrdersResponse> GetOpenOrdersAsync();
        Task<AcceptOrderResponse> AcceptOrderAsync(string taxiCompanyId, int orderId);
    }
}