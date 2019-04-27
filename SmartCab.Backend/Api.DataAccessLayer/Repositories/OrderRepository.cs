using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Statuses;
using CustomExceptions;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationContext _context;

        public OrderRepository(ApplicationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all open orders with status WaitingForAccept
        /// </summary>
        /// <returns>Returns open orders. </returns>
        public Task<List<Order>> GetOpenOrdersAsync()
        {
            var orders = _context.Orders
                .Where(x => x.Status == OrderStatus.WaitingForAccept) //TODO: Change this method
                .ToListAsync();
            return orders;
        }

        /// <summary>
        /// Update the order to accepted, and who accepted the taxicompany. 
        /// </summary>
        /// <param name="taxicompanyId">Taxicompany which accepted order</param>
        /// <param name="orderId">Id of the accepted order</param>
        /// <returns>Returns the updated order</returns>
        public async Task<Order> AcceptOrderAsync(string taxicompanyId, int orderId)
        {
            var taxiCompanyTask =  FindTaxiCompanyAsync(taxicompanyId);
            var order = await FindOrderAsync(orderId);
            SetOrderToAccepted(order);

            //Set status on orders connected rides. 
            SetAllRidesToAccepted(order.Rides);

            //Set company that accepted
            order.TaxiCompany = await taxiCompanyTask;


            //Save
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        /// <summary>
        /// Returns the order with the supplied id.
        /// </summary>
        /// <param name="orderId">The id of the order.</param>
        /// <returns>The order that has the supplied id.</returns>
        private async Task<Order> FindOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new UserIdInvalidException("Order does not exists"); // Exchange with custom error. 
            }

            return order;
        }

        /// <summary>
        /// Returns the taxi company that has the supplied id.
        /// </summary>
        /// <param name="taxicompanyId">The id of the taxi company.</param>
        /// <returns>The taxi company that has the supplied id.</returns>
        private async Task<TaxiCompany> FindTaxiCompanyAsync(string taxicompanyId)
        {
            var taxiCompany = await _context.TaxiCompanies.FindAsync(taxicompanyId);
            if (taxiCompany == null)
            {
                throw new UserIdInvalidException("TaxiCompany does not exists");
            }

            return taxiCompany;
        }

        /// <summary>
        /// Updates the status of the order to "Accepted".
        /// </summary>
        /// <param name="order">The order that should have its status updated.</param>
        private void SetOrderToAccepted(Order order)
        {
            if (order.Status != OrderStatus.WaitingForAccept)
            {
                throw new UnexpectedStatusException("Order is not waiting for accept, cannot be accepted");
            }
            //Set status
            order.Status = OrderStatus.Accepted;
        }

        /// <summary>
        /// Updates the status of all supplied rides to "Accepted".
        /// </summary>
        /// <param name="rides">The collection of rides, that should have their status updated.</param>
        private void SetAllRidesToAccepted(List<Ride> rides)
        {
            foreach (var ride in rides)
            {
                if (ride.Status != RideStatus.WaitingForAccept)
                {
                    throw new UnexpectedStatusException("Ride is not waiting for accept, cannot be accepted");
                }
                ride.Status = RideStatus.Accepted;
            }
        }
    }
}