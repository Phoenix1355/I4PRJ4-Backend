using System.Threading.Tasks;
using Api.BusinessLogicLayer.Responses;

namespace Api.BusinessLogicLayer.Interfaces
{
    public interface IGoogleMapsApiService
    {
        Task<decimal> GetDistanceInKm(string origin, string destination);
        Task ValidateAddress(string address);
    }
}