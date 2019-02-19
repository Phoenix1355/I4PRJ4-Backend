using System.Collections.Generic;
using System.Threading.Tasks;
using SmartCabPoC.DataLayer.Models;

namespace SmartCabPoC.BusinessLayer.Abstractions
{
    public interface IRideService
    {
        Task<List<Ride>> GetAllRidesAsync();
        Task AddRideAsync(Ride ride);
    }
}