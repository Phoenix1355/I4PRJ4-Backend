﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Migrations;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Statuses;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.Repositories
{
    /// <summary>
    /// This class exposes all the possible request to the database that is related to "Rides"
    /// </summary>
    public class RideRepository : IRideRepository, IDisposable
    {
        private readonly ApplicationContext _context;

        public RideRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<Ride>> GetAllRidesAsync()
        {
            var rides = await _context.Rides.ToListAsync();
            return rides;
        }

        /// <summary>
        /// Returns all SoloRides with status WaitingForAccept
        /// </summary>
        /// <returns></returns>
        public Task<List<SoloRide>> GetOpenSoloRidesAsync()
        {
            var rides = _context.SoloRides
                .Where(x=>x.Status == RideStatus.Expired) //TODO: Change this method
                .ToListAsync();
            return rides;
        }

        public async Task<Ride> CreateSharedRideAsync(Ride ride)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                //Add ride and reserves amount.
                ride = await AddRideAndReserveFundsForRide(ride);

                //Reserve from Customer
                transaction.Commit();
                return ride;
            }
        }

        public async Task<Ride> CreateSoloRideAsync(Ride ride)
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

        private async Task<Ride> AddRideAndReserveFundsForRide(Ride ride)
        {
            //Reserve funds. 
            await ReservePriceFromCustomer(ride.CustomerId, ride.Price);

            //adds SoloRide
            return ride = await AddRide(ride);
        }

        private async Task ReservePriceFromCustomer(string CustomerId, decimal price)
        {
            var customer = await _context.Customers.FindAsync(CustomerId);
            if ((customer.Balance - customer.ReservedAmount) > price)
            {
                customer.ReservedAmount -= price;
                _context.Customers.Update(customer);
                _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentOutOfRangeException("Not enough credit");
            }
        }

        private async Task<Ride> AddRide(Ride ride)
        {
            _context.Rides.AddAsync(ride);
            _context.SaveChangesAsync();
            return ride;
        }

        private async Task<Order> AddOrderFromRide(Ride ride)
        {
            Order order = new Order()
            {
                Price = ride.Price,
                Rides = new List<Ride>(),
                Status = OrderStatus.WaitingForAccept,
            };
            order.Rides.Add(ride);

            _context.Orders.AddAsync(order);
            _context.SaveChangesAsync();
            return order;
        }





        public async Task<Ride> UpdateRideAsync(Ride ride)
        {
            _context.Rides.Update(ride);
            await _context.SaveChangesAsync();
            return ride;
        }

        public async Task<Ride> DeleteRideAsync(int id)
        {
            var ride = await _context.Rides.SingleOrDefaultAsync(r => r.Id == id);
            _context.Rides.Remove(ride);
            await _context.SaveChangesAsync();
            return ride;
        }

        #region IDisposable implementation

        //Dispose pattern:
        //https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose#basic_pattern
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _context?.Dispose();
        }

        #endregion
    }
}