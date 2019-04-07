using System.Collections.Generic;
using Newtonsoft.Json;

namespace Api.BusinessLogicLayer.Responses
{
    /// <summary>
    /// This class, and the classes within it is used when deserializing a geocoding response from google.
    /// <remarks>
    /// Generated using http://json2csharp.com/
    /// </remarks>
    /// </summary>
    public class GoogleGeocodingResponse
    {
        public List<Result> Results { get; set; }
        public string Status { get; set; }

        public class AddressComponent
        {
            [JsonProperty(PropertyName = "long_name")]
            public string LongName { get; set; }
            [JsonProperty(PropertyName = "short_name")]
            public string ShortName { get; set; }
            public List<string> Types { get; set; }
        }

        public class Location
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
        }

        public class Northeast
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
        }

        public class Southwest
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
        }

        public class Viewport
        {
            public Northeast Northeast { get; set; }
            public Southwest Southwest { get; set; }
        }

        public class Geometry
        {
            public Location Location { get; set; }
            [JsonProperty(PropertyName = "location_type")]
            public string LocationType { get; set; }
            public Viewport Viewport { get; set; }
        }

        public class Result
        {
            [JsonProperty(PropertyName = "address_components")]
            public List<AddressComponent> AddressComponents { get; set; }
            [JsonProperty(PropertyName = "formatted_address")]
            public string FormattedAddress { get; set; }
            public Geometry Geometry { get; set; }
            [JsonProperty(PropertyName = "place_id")]
            public string PlaceId { get; set; }
            public List<string> Types { get; set; }
        }
    }
}