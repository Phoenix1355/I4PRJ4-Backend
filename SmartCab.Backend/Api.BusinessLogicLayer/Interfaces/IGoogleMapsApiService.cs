using System.Threading.Tasks;
using Api.DataAccessLayer.Models;

namespace Api.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Interface for google maps API for testability. 
    /// </summary>
    public interface IGoogleMapsApiService
    {
        Task<decimal> GetDistanceInKmAsync(string origin, string destination);
        Task ValidateAddressAsync(Address address);
    }
}