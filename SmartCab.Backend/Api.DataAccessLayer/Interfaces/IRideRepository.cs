using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DataAccessLayer.Models;

namespace Api.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Defines the interface to the database in relation to "Rides".
    /// </summary>
    public interface IRideRepository
    {
        Task<SharedRide> CreateSharedRideAsync(SharedRide ride);
        SoloRide AddSoloRideAsync(SoloRide ride);
    }
}