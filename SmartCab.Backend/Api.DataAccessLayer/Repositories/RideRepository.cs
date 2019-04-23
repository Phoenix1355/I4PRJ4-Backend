﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Migrations;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Statuses;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using CustomExceptions;
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
                .Where(x => x.Status == RideStatus.Expired) //TODO: Change this method
                .ToListAsync();
            return rides;
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
                throw new Exception("Order does not exists"); // Exchange with custom error. 
            }

            return order;
        }

        private async Task<TaxiCompany> FindTaxiCompany(string taxicompanyId)
        {
            var taxiCompany = await _context.TaxiCompanies.FindAsync(taxicompanyId);
            if (taxiCompany == null)
            {
                throw new Exception("Order does not exists"); // Exchange with custom error. 
            }

            return taxiCompany;
        }

        private void SetOrderToAccepted(Order order)
        {
            if (order.Status != OrderStatus.WaitingForAccept)
            {
                throw new Exception("Order is not waiting for accept, cannot be accepted"); // Change with custom error. 
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
                    throw new Exception("Order is not waiting for accept, cannot be accepted"); // Change with custom error. 
                }
                ride.Status = RideStatus.Accepted;
            }
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