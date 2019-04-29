using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Statuses;
using Api.DataAccessLayer.UnitOfWork;
using CustomExceptions;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.Repositories
{
    /// <summary>
    /// Handles all actions involving orders. 
    /// </summary>
    public class OrderRepository : GenericRepository<Order>,IOrderRepository
    {
        /// <summary>
        /// Constructor for order repository. 
        /// </summary>
        /// <param name="context"></param>
        public OrderRepository(ApplicationContext context) : base(context)
        {
        }

        /// <summary>
        /// Add a ride to an order 
        /// </summary>
        /// <param name="ride"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="MultipleOrderException">Already an order for given ride.</exception> 
        public Order AddRideToOrder(Ride ride, Order order)
        {
            
            if (Find(o => o.Rides.Contains(ride)).Count != 0)
            {
                throw new MultipleOrderException("Already an order for given ride. ");
            }

            order.Price += ride.Price;
            order.Rides.Add(ride);
            return Update(order);
        }

        /// <summary>
        /// Find all orders with status waiting for accept. 
        /// </summary>
        /// <returns>List of orders with status waiting for accept.</returns>
        public List<Order> FindOpenOrders()
        {
            return Find(order => order.Status == OrderStatus.WaitingForAccept);
        }
    }
}