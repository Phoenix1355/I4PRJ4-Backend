using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace Api.BusinessLogicLayer.Services
{
    /// <summary>
    /// Source: https://dujushi.github.io/2016/01/12/Google-Distance-Matrix-API-with-HttpClient-and-Json-NET/
    /// </summary>
    public class GoogleDistanceMatrixService
    {
        private readonly string _url;
        private readonly string _key;
        public string[] OriginAddresses { get; set; }
        public string[] DestinationAddresses { get; set; }

        public GoogleDistanceMatrixService(string[] originAddresses, string[] destinationAddresses)
        {
            _url = "https://maps.googleapis.com/maps/api/distancematrix/json";
            _key = "AIzaSyDRSgpNi8u5jgZI5vcBfzSmZLDXKADlYCQ";
            OriginAddresses = originAddresses;
            DestinationAddresses = destinationAddresses;
        }

        private string GetRequestUrl()
        {
            OriginAddresses = OriginAddresses.Select(HttpUtility.UrlEncode).ToArray();
            var origins = string.Join("|", OriginAddresses);
            DestinationAddresses = DestinationAddresses.Select(HttpUtility.UrlEncode).ToArray();
            var destinations = string.Join("|", DestinationAddresses);
            return $"{_url}?origins={origins}&destinations={destinations}&key={_key}";
        }

        public async Task<Response> GetResponse()
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(GetRequestUrl());

                HttpResponseMessage response = await client.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("GoogleDistanceMatrixApi failed with status code: " + response.StatusCode);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Response>(content);
                }
            }
        }
    }

    public class Response
    {
        public string Status { get; set; }

        [JsonProperty(PropertyName = "origin_addresses")]
        public string[] OriginAddresses { get; set; }

        [JsonProperty(PropertyName = "destination_addresses")]
        public string[] DestinationAddresses { get; set; }

        public Row[] Rows { get; set; }

        public class Data
        {
            public int Value { get; set; }
            public string Text { get; set; }
        }

        public class Element
        {
            public string Status { get; set; }
            public Data Duration { get; set; }
            public Data Distance { get; set; }
        }

        public class Row
        {
            public Element[] Elements { get; set; }
        }
    }
}