using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Responses;

namespace Api.BusinessLogicLayer.Interfaces
{
    public interface IOrderService
    {
        Task<OpenOrdersResponse> GetOpenOrdersAsync();
        Task<AcceptOrderResponse> AcceptOrderAsync(string taxiCompanyId, int orderId);

        /// <summary>
        /// Returns an orderDto containing all key information about order. 
        /// </summary>
        /// <returns>An object containing all open orders stored in the system</returns>
        Task<OrderDetailedDto> GetOrderAsync(int orderId);
    }
}