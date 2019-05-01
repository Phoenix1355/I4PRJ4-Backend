using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;

namespace Api.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Interface for IOrderRepository
    /// </summary>
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> AddRideToOrderAsync(Ride ride, Order order);
        Task<List<Order>> FindOpenOrdersAsync();
        Order SetOrderToAccepted(Order order, string taxiCompanyId);
        void SetOrderToDebited(Order order);
    }
}