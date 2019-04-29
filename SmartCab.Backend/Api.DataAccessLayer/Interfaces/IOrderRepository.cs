using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DataAccessLayer.Models;

namespace Api.DataAccessLayer.Interfaces
{
    public interface IOrderRepository
    {
        Order AddRideToOrder(Ride ride, Order order);
    }
}