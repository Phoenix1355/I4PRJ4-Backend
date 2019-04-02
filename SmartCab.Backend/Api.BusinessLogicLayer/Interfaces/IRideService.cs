using System.Collections.Generic;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Models;

namespace Api.BusinessLogicLayer.Interfaces
{
    public interface IRideService
    {
        Task<OpenRidesResponse> GetAllOpenRidesAsync();
        //Task<List<Ride>> GetAllRidesAsync();
        //Task<Ride> GetRideByIdAsync(int id);
        //Task<Ride> AddRideAsync(Ride ride);
        //Task<Ride> UpdateRideAsync(Ride ride);
        //Task<Ride> DeleteRideAsync(int id);
    }
}