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
        Order AddRideToOrder(Ride ride, Order order);
        List<Order> FindOpenOrders();
    }
}