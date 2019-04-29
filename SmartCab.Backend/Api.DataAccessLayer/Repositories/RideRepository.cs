using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Migrations;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Statuses;
using Api.DataAccessLayer.UnitOfWork;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using CustomExceptions;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.Repositories
{
    /// <summary>
    /// This class exposes all the possible request to the database that is related to "Rides"
    /// </summary>
    public class RideRepository : GenericRepository<Ride>, IRideRepository
    {
        /// <summary>
        /// Constructor for Ride Repository. 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public RideRepository(ApplicationContext context) : base(context)
        {

        }
        
        public Task<List<SoloRide>> GetOpenSoloRidesAsync()
        {
            var rides = _context.SoloRides
                .Where(x => x.Status == RideStatus.Expired) //TODO: Change this method
                .ToListAsync();
            return rides;
        }

        public async Task<SharedRide> CreateSharedRideAsync(SharedRide ride)
        {
            throw new NotImplementedException();
            using (var transaction = _context.Database.BeginTransaction())
            {
                //Add ride and reserves amount.
                //ride = await AddRideAndReserveFundsForRide(ride);

                ////Reserve from Customer
                //transaction.Commit();
                //return ride;
            }
        }

        public async Task<SoloRide> AddSoloRideAsync(SoloRide ride)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                //Add ride and reserves amount.
                ride = await AddRideAndReserveFundsForRide(ride);

                //Adds order
                await AddOrderFromRide(ride);

                //Reserve from Customer
                transaction.Commit();
                return ride;
            }
        }

        private async Task<SoloRide> AddRideAndReserveFundsForRide(SoloRide ride)
        {
            //Reserve funds. 
            ReservePriceFromCustomer(ride.CustomerId, ride.Price);

            //adds SoloRide
            return await AddRide(ride);
        }

        private void ReservePriceFromCustomer(string customerId, decimal price)
        {
            var customer = FindsCustomerElseThrow(customerId);

            if ((customer.Balance - customer.ReservedAmount) >= price)
            {
                customer.ReservedAmount += price;
                _context.Customers.Update(customer);
                _context.SaveChanges();
            }
            else
            {
                throw new InsufficientFundsException("Not enough credit");
            }
        }

        private Customer FindsCustomerElseThrow(string customerId)
        {
            var customer = _context.Customers.Find(customerId);
            return customer;
        }

        private async Task<SoloRide> AddRide(SoloRide ride)
        {
            _context.Rides.Update(ride);
            await _context.SaveChangesAsync();
            return ride;
        }

        private async Task<Order> AddOrderFromRide(Ride ride)
        {
            if (_context.Orders.Count(o => o.Rides.Contains(ride)) != 0)
            {
                throw new MultipleOrderException("Already an order for given ride. ");
            }

            Order order = new Order()
            {
                Price = ride.Price,
                Rides = new List<Ride>(),
                Status = OrderStatus.WaitingForAccept,
            };
            order.Rides.Add(ride);

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }
    }
}