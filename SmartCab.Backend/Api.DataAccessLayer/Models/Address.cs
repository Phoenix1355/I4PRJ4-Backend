using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.Models
{
    [Owned]
    public class Address
    {
        public string CityName { get; set; }
        public int PostalCode { get; set; }
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }

        public Address(string cityName, int postalCode, string streetName, int streetNumber)
        {
            CityName = cityName;
            PostalCode = postalCode;
            StreetName = streetName;
            StreetNumber = streetNumber;
        }
    }
}