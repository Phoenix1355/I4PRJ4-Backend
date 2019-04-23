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
        public async Task<Order> AcceptOrder(string taxicompanyId, int orderId)
        {
            var order = await FindOrder(orderId);
            SetOrderToAccepted(order);

            //Set status on orders connected rides. 
            SetAllRidesToAccepted(order.Rides);

            //Set company that accepted
            var taxiCompany = await FindTaxiCompany(taxicompanyId);
            order.TaxiCompany = taxiCompany;


            //Save
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        private async Task<Order> FindOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new UserIdInvalidException("Order does not exists"); // Exchange with custom error. 
            }

            return order;
        }

        private async Task<TaxiCompany> FindTaxiCompany(string taxicompanyId)
        {
            var taxiCompany = await _context.TaxiCompanies.FindAsync(taxicompanyId);
            if (taxiCompany == null)
            {
                throw new UserIdInvalidException("TaxiCompany does not exists");
            }

            return taxiCompany;
        }

        private void SetOrderToAccepted(Order order)
        {
            if (order.Status != OrderStatus.WaitingForAccept)
            {
                throw new UnexpectedStatusException("Order is not waiting for accept, cannot be accepted");
            }
            //Set status
            order.Status = OrderStatus.Accepted;
        }

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