using System.Threading.Tasks;
using Api.BusinessLogicLayer.Responses;

namespace Api.BusinessLogicLayer.Interfaces
{
    public interface IGoogleMapsApiService
    {
        Task<GoogleMapsApiResponse> GetDistance(string[] originAddresses, string[] destinationAddresses);
    }
}