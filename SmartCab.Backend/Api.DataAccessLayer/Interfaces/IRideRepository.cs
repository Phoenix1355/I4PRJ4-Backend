using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;

namespace Api.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Defines the interface to the database in relation to "Rides".
    /// </summary>
    public interface IRideRepository : IGenericRepository<Ride>
    {
        /// <summary>
        /// Updates the status of all supplied rides to "Accepted".
        /// </summary>
        /// <param name="rides">The collection of rides, that should have their status updated.</param>
        void SetAllRidesToAccepted(List<Ride> rides);

        Task<List<Ride>> FindUnmatchedSharedRides();
    }
}