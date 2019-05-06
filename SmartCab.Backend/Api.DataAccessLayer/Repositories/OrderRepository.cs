using System;
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
        /// <param name="context">The context used to access the database.</param>
        public OrderRepository(ApplicationContext context) : base(context)
        {
        }

        /// <summary>
        /// Find all orders that have a ride which is expired. Includes both shared and solo rides. 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Order>> FindOrdersWithExpiredRides()
        {
            return await FindAsync((order) =>
                order.Rides.Where(
                    (ride) => (ride.ConfirmationDeadline < DateTime.Now && ride.Status == RideStatus.WaitingForAccept)
                ).Any() &&
                order.Status == OrderStatus.WaitingForAccept);
        }


        /// <summary>
        /// Adds a ride to an order asynchronously.
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

            ride.Status = RideStatus.WaitingForAccept;
            order.Price += ride.Price;
            order.Rides.Add(ride);
            Update(order);
            return order;
        }

        /// <summary>
        /// Finds all orders with the status waiting for accept. 
        /// </summary>
        /// <returns>List of orders with status waiting for accept.</returns>
        public Task<List<Order>> FindOpenOrdersAsync()
        {
            return FindAsync(order => order.Status == OrderStatus.WaitingForAccept);
        }

        /// <summary>
        /// Updates the status of the order to "Accepted".
        /// </summary>
        /// <param name="order">The order that should have its status updated.</param>
        /// <param name="taxiCompanyId">The taxicompany that accepted the order.</param>
        /// <exception cref="UnexpectedStatusException">Order is not waiting for accept, cannot be accepted.</exception> 
        public Order SetOrderToAccepted(Order order, string taxiCompanyId)
        {
            if (order.Status != OrderStatus.WaitingForAccept)
            {
                throw new UnexpectedStatusException("Order is not waiting for accept, cannot be accepted");
            }
            //Set status
            order.TaxiCompanyId = taxiCompanyId;
            order.Status = OrderStatus.Accepted;
            return Update(order);
        }

        /// <summary>
        /// Changes the status to Debited if order is Accepted
        /// </summary>
        /// <param name="order">Order to change status on</param>
        /// /// <exception cref="UnexpectedStatusException">Order is not accepted, cannot be debited.</exception> 
        public void SetOrderToDebited(Order order)
        {
            //Validate that order is accepted
            if (order.Status != OrderStatus.Accepted)
            {
                throw new UnexpectedStatusException("Order is not accepted, cannot be debited");
            }
            order.Status = OrderStatus.Debited;

            Update(order);
        }
    }
}