using System.Collections.Generic;
using Newtonsoft.Json;

namespace Api.BusinessLogicLayer.Responses
{
    /// <summary>
    /// This class, and the classes within it is used when deserializing a distance matrix response from google.
    /// <remarks>
    /// Generated using http://json2csharp.com/
    /// </remarks>
    /// </summary>
    public class GoogleDistanceMatrixResponse
    {
        [JsonProperty(PropertyName = "destination_addresses")]
        public List<string> DestinationAddresses { get; set; }
        [JsonProperty(PropertyName = "origin_addresses")]
        public List<string> OriginAddresses { get; set; }
        public List<Row> Rows { get; set; }
        public string Status { get; set; }

        public class Distance
        {
            public string Text { get; set; }
            public int Value { get; set; }
        }

        public class Duration
        {
            public string Text { get; set; }
            public int Value { get; set; }
        }

        public class Element
        {
            public Distance Distance { get; set; }
            public Duration Duration { get; set; }
            public string Status { get; set; }
        }

        public class Row
        {
            public List<Element> Elements { get; set; }
        }
    }
}