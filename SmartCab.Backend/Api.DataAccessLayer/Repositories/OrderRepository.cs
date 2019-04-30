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
        /// AddAsync a ride to an order 
        /// </summary>
        /// <param name="ride"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="MultipleOrderException">Already an order for given ride.</exception> 
        public async Task<Order> AddRideToOrderAsync(Ride ride, Order order)
        {
            var orders = await FindAsync(o => o.Rides.Contains(ride));
            if (orders.Count != 0)
            {
                throw new MultipleOrderException("Already an order for given ride. ");
            }

            order.Price += ride.Price;
            order.Rides.Add(ride);
            return await UpdateAsync(order);
        }

        /// <summary>
        /// FindAsync all orders with status waiting for accept. 
        /// </summary>
        /// <returns>List of orders with status waiting for accept.</returns>
        public async Task<List<Order>> FindOpenOrdersAsync()
        {
            return await FindAsync(order => order.Status == OrderStatus.WaitingForAccept);
        }

        /// <summary>
        /// Updates the status of the order to "Accepted".
        /// </summary>
        /// <param name="order">The order that should have its status updated.</param>
        /// <param name="taxiCompanyId">The taxicompany that accepted the order.</param>
        public Task<Order> SetOrderToAccepted(Order order, string taxiCompanyId)
        {
            if (order.Status != OrderStatus.WaitingForAccept)
            {
                throw new UnexpectedStatusException("Order is not waiting for accept, cannot be accepted");
            }
            //Set status
            order.TaxiCompanyId = taxiCompanyId;
            order.Status = OrderStatus.Accepted;
            return UpdateAsync(order);
        }
    }
}