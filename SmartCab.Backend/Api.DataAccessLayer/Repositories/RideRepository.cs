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

        /// <summary>
        /// Updates the status of all supplied rides to "Accepted".
        /// </summary>
        /// <param name="rides">The collection of rides, that should have their status updated.</param>
        /// <exception cref="UnexpectedStatusException">Ride is not waiting for accept, cannot be accepted.</exception> 
        public async Task SetAllRidesToAccepted(List<Ride> rides)
        {
            foreach (var ride in rides)
            {
                if (ride.Status != RideStatus.WaitingForAccept)
                {
                    throw new UnexpectedStatusException("Ride is not waiting for accept, cannot be accepted");
                }
                ride.Status = RideStatus.Accepted;
                await UpdateAsync(ride);
            }
        }
    }
}