using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Responses;
using CustomExceptions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Api.BusinessLogicLayer.Services
{
    /// <summary>
    /// Source: https://dujushi.github.io/2016/01/12/Google-Distance-Matrix-API-with-HttpClient-and-Json-NET/
    /// </summary>
    public class GoogleMapsApiService : IGoogleMapsApiService
    {
        private readonly string _distanceMatrixUrl;
        private readonly string _geocodingUrl;
        private readonly string _apiKey;

        public GoogleMapsApiService(IConfiguration config)
        {
            _distanceMatrixUrl = "https://maps.googleapis.com/maps/api/distancematrix/json";
            _geocodingUrl = "https://maps.googleapis.com/maps/api/geocode/json";
            _apiKey = config["GoogleMapsApiKey"];
        }

        public async Task<decimal> GetDistanceInKm(string origin, string destination)
        {
            using(var client = new HttpClient())
            {
                var uri = new Uri(GetDistanceMatrixRequestUrl(origin, destination));

                var response = await client.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("GoogleDistanceMatrixApi failed with status code: " + response.StatusCode);
                }

                var content = await response.Content.ReadAsStringAsync();
                var distanceMatrix = JsonConvert.DeserializeObject<GoogleDistanceMatrixResponse>(content);
                
                //Distance is returned in meters, so we divide with 1000 to get the result in kilometers
                var distanceInKm = Convert.ToDecimal(distanceMatrix
                                                     .Rows
                                                     .FirstOrDefault()?
                                                     .Elements.FirstOrDefault()?
                                                     .Distance.Value / 1000.0);

                if (distanceMatrix.Status != "OK" || distanceInKm <= 0)
                {
                    throw new GoogleMapsApiException("A route between the provided addresses could not be calculated.");
                }

                return distanceInKm;
            }
        }

        public async Task ValidateAddress(string address)
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(GetGeocodingRequestUrl(address));
                var response = await client.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("GoogleDistanceMatrixApi failed with status code: " + response.StatusCode);
                }

                var content = await response.Content.ReadAsStringAsync();
                var geocodingResponse = JsonConvert.DeserializeObject<GoogleGeocodingResponse>(content);
                var locationType = geocodingResponse
                                   .Results.FirstOrDefault()?
                                   .Geometry
                                   .LocationType;

                //"ROOFTOP" indicates the address is precise down to street level
                //"RANGE_INTERPOLATED" indicates the result reflects an approximation (usually on a road)
                //interpolated between two precise points (such as intersections). This is good enough
                //https://developers.google.com/maps/documentation/geocoding/intro#reverse-restricted
                if (locationType != "ROOFTOP" && locationType != "RANGE_INTERPOLATED")
                {
                    throw new GoogleMapsApiException($"The address \"{address}\" is not valid.");
                }
            }
        }

        private string GetDistanceMatrixRequestUrl(string origin, string destination)
        {
            return $"{_distanceMatrixUrl}?origins={origin}&destinations={destination}&region=dk&key={_apiKey}";
        }

        private string GetGeocodingRequestUrl(string address)
        {
            return $"{_geocodingUrl}?address={address}&key={_apiKey}";
        }
    }
}