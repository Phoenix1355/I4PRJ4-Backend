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
        private readonly string _url;
        private readonly string _key;

        public GoogleMapsApiService(IConfiguration config)
        {
            _url = "https://maps.googleapis.com/maps/api/distancematrix/json";
            _key = config["GoogleMapsApiKey"];
        }

        public async Task<GoogleMapsApiResponse> GetDistance(string[] originAddresses, string[] destinationAddresses)
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(GetRequestUrl(originAddresses, destinationAddresses));

                var response = await client.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("GoogleDistanceMatrixApi failed with status code: " + response.StatusCode);
                }

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GoogleMapsApiResponse>(content);
            }
        }

        private string GetRequestUrl(string[] originAddresses, string[] destinationAddresses)
        {
            originAddresses = originAddresses.Select(HttpUtility.UrlEncode).ToArray();
            var origins = string.Join("|", originAddresses);
            destinationAddresses = destinationAddresses.Select(HttpUtility.UrlEncode).ToArray();
            var destinations = string.Join("|", destinationAddresses);
            return $"{_url}?origins={origins}&destinations={destinations}&region=dk&key={_key}";
        }
    }
}