using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Responses;

namespace Api.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// The possible function within the order services. 
    /// </summary>
    public interface IOrderService
    {
        Task<OpenOrdersResponse> GetOpenOrdersAsync();
        Task<AcceptOrderResponse> AcceptOrderAsync(string taxiCompanyId, int orderId);
        Task<OrderDetailedDto> GetOrderAsync(int orderId);
    }
}