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
    public class RideRepository : GenericRepository<Ride>,IRideRepository
    {
        public RideRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<SharedRide> CreateSharedRideAsync(SharedRide ride)
        {
            throw new NotImplementedException();
            
        }

        public SoloRide AddSoloRideAsync(SoloRide ride)
        {
            _unitOfWork.ReservePriceFromCustomer(ride.CustomerId, ride.Price);
            ride = (SoloRide)_unitOfWork.GenericRideRepository.Add(ride);
            var order = _unitOfWork.GenericOrderRepository.Add(new Order());
            _unitOfWork.AddRideToOrder(ride, order);
            //_unitOfWork.SaveChanges();
            return ride;
        }


    }
}