using System;
using System.ComponentModel;
using System.Diagnostics;
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
    /// This class is used to send requests to the Google Maps Api.
    /// </summary>
    /// <remarks>
    /// Two services are used:<br/>
    ///     Distance Matrix<br/>
    ///     Geocoding<br/>
    /// A secret api key must be used to be allowed to make requests to those endpoint.<br/>
    /// This api key is stored in Azure under the key "GoogleMapsApiKey".<br/>
    /// Inspiration gotten from: https://dujushi.github.io/2016/01/12/Google-Distance-Matrix-API-with-HttpClient-and-Json-NET/
    /// </remarks>
    public class GoogleMapsApiService : IGoogleMapsApiService
    {
        private readonly string _distanceMatrixUrl;
        private readonly string _geocodingUrl;
        private readonly string _apiKey;
        private readonly IHttpClient _client;

        /// <summary>
        /// Constructor for this class.
        /// </summary>
        /// <param name="config">The application configuration.</param>
        /// <param name="client">The http client used to send requests to the Google Maps Api.</param>
        public GoogleMapsApiService(IConfiguration config, IHttpClient client)
        {
            _distanceMatrixUrl = config["DistanceMatrixUrl"];
            _geocodingUrl = config["GeocodingUrl"];
            _apiKey = config["GoogleMapsApiKey"]; //when run locally, secrets.json will be used
            _client = client;
        }

        /// <summary>
        /// Sends a request to google maps api's distance matrix endpoint and returns the distance between two addresses.
        /// </summary>
        /// <param name="origin">The origin</param>
        /// <param name="destination">The destination</param>
        /// <returns>The distance between origin and destination in kilometers.</returns>
        public async Task<decimal> GetDistanceInKmAsync(string origin, string destination)
        {

            var uri = new Uri(GetDistanceMatrixRequestUrl(origin, destination));
            var response = await _client.GetAsync(uri);
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

        /// <summary>
        /// Validates an address. Throws a GoogleMapsApiException if validation fails.
        /// </summary>
        /// <remarks>
        /// Validation fails if the precision of the address was not down to street level.<br/>
        /// The validation will for example fail if the street number is missing.
        /// </remarks>
        /// <param name="address">The address that should be validated.</param>
        public async Task ValidateAddressAsync(string address)
        {
            var uri = new Uri(GetGeocodingRequestUrl(address));
            var response = await _client.GetAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("GoogleDistanceMatrixApi failed with status code: " + response.StatusCode);
            }

                var content = await response.Content.ReadAsStringAsync();
                var geocodingResponse = JsonConvert.DeserializeObject<GoogleGeocodingResponse>(content);

                if (geocodingResponse.Status != "OK")
                {
                    Debug.WriteLine("The response from the Google Maps Api was invalid. Most likely due to an invalid API key.");
                    throw new Exception(
                        "The response from the Google Maps Api was invalid. Most likely due to an invalid API key.");
                }

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

        /// <summary>
        /// Returns an url for the distance matrix endpoint.
        /// </summary>
        /// <param name="origin">The origin</param>
        /// <param name="destination">The destination</param>
        /// <returns>The url.</returns>
        private string GetDistanceMatrixRequestUrl(string origin, string destination)
        {
            return $"{_distanceMatrixUrl}?origins={origin}&destinations={destination}&region=dk&key={_apiKey}";
        }

        /// <summary>
        /// Returns an url for the geocoding endpoint.
        /// </summary>
        /// <param name="address">An address</param>
        /// <returns>The url.</returns>
        private string GetGeocodingRequestUrl(string address)
        {
            return $"{_geocodingUrl}?address={address}&key={_apiKey}";
        }
    }
}