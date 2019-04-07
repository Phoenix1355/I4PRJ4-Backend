using System.Collections.Generic;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Models;

namespace Api.BusinessLogicLayer.Interfaces
{
    public interface IRideService
    {
        //Task<List<Ride>> GetAllRidesAsync();
        //Task<Ride> GetRideByIdAsync(int id);
        Task<CreateRideResponse> AddRideAsync(CreateRideRequest request, string customerId);
        Task<decimal> CalculatePriceAsync(Address first, Address second, bool isShared);
        //Task<Ride> UpdateRideAsync(Ride ride);
        //Task<Ride> DeleteRideAsync(int id);
    }
}