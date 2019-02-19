using System.Collections.Generic;
using System.Threading.Tasks;
using Ride = SmartCabPoC.DataLayer.Models.Ride;

namespace SmartCabPoC.DataLayer.Abstractions
{
    public interface IRideRepository
    {
        Task<List<Ride>> GetAllRidesAsync();
        Task AddRide(Ride ride);
    }
}