using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;

namespace Api.Integration.Test.Fakes
{
    /// <summary>
    /// Source: https://dujushi.github.io/2016/01/12/Google-Distance-Matrix-API-with-HttpClient-and-Json-NET/
    /// </summary>
    public class FakeGoogleMapsApiService : IGoogleMapsApiService
    {
        private readonly string _distanceMatrixUrl;
        private readonly string _geocodingUrl;
        private readonly string _apiKey;

        public FakeGoogleMapsApiService(IConfiguration config)
        {
            _distanceMatrixUrl = "https://maps.googleapis.com/maps/api/distancematrix/json";
            _geocodingUrl = "https://maps.googleapis.com/maps/api/geocode/json";
            _apiKey = config["GoogleMapsApiKey"];
        }

        //public async Task<decimal> GetDistanceInKm(string origin, string destination)
        //{
        //    return 10;
        //}

        //public async Task ValidateAddress(string address)
        //{
        //    return;
        //}

        public Task<decimal> GetDistanceInKmAsync(string origin, string destination)
        {
            return Task.Run(() => (decimal)10);
        }

        public async Task ValidateAddressAsync(Address address)
        {
            return;
        }
    }
}