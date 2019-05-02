using System.Threading.Tasks;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Models;

namespace Api.BusinessLogicLayer.Interfaces
{
    public interface IGoogleMapsApiService
    {
        Task<decimal> GetDistanceInKmAsync(string origin, string destination);
        Task ValidateAddressAsync(Address address);
    }
}