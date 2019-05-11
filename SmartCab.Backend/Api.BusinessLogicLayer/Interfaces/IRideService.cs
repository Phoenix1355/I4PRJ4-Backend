using System.Threading.Tasks;
using Api.BusinessLogicLayer.Enums;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Models;

namespace Api.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Interface for all ride functions. 
    /// </summary>
    public interface IRideService
    {
        Task<CreateRideResponse> AddRideAsync(CreateRideRequest request, string customerId);
        Task<decimal> CalculatePriceAsync(Address first, Address second, RideType type);
    }
}