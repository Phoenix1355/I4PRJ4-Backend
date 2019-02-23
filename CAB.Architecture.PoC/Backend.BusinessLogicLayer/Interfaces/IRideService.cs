using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DataAccessLayer.Models;

namespace Backend.BusinessLogicLayer.Interfaces
{
    public interface IRideService
    {
        Task<List<Ride>> GetAllRidesAsync();
        Task AddRideAsync(Ride ride);
    }
}