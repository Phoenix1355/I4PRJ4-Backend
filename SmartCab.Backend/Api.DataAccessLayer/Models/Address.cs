using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.Models
{
    /// <summary>
    /// Address table in database. Is not selfcontained in a table, but is mapped in Ride. 
    /// </summary>
    [Owned]
    public class Address
    {
        public int Id { get; set; }
        [Required]
        public string CityName { get; set; }
        [Required]
        public int PostalCode { get; set; }
        [Required]
        public string StreetName { get; set; }
        [Required]
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