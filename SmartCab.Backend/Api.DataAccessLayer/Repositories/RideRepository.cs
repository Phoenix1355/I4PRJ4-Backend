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
    public class RideRepository : IRideRepository, IDisposable
    {
        private readonly IUoW _unitOfWork;

        public RideRepository(IUoW unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<SharedRide> CreateSharedRideAsync(SharedRide ride)
        {
            throw new NotImplementedException();
            using (var transaction = _unitOfWork.Database.BeginTransaction())
            {
                //Add ride and reserves amount.
                //ride = await AddRideAndReserveFundsForRide(ride);

                ////Reserve from Customer
                //transaction.Commit();
                //return ride;
            }
        }

        public SoloRide AddSoloRideAsync(SoloRide ride)
        {
            _unitOfWork.ReservePriceFromCustomer(ride.CustomerId, ride.Price);
            ride = (SoloRide)_unitOfWork.RideRepository.Add(ride);
            var order = _unitOfWork.OrderRepository.Add(new Order());
            _unitOfWork.AddRideToOrder(ride, order);
            _unitOfWork.SaveChanges();
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
            _unitOfWork?.Dispose();
        }

        #endregion
    }
}