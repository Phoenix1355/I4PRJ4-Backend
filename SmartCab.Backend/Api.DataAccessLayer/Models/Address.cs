using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.Models
{
    /// <summary>
    /// Represents an address.
    /// </summary>
    /// <remarks>
    /// Marked with the [Owned] annotation which means that no table will representing addresses will be created in the database.<br/>
    /// Instead other classes references this class using navigational properties.<br/>
    /// The classes that has an address as navigational property (eg. ride's StartDestination)<br/>
    /// get additional columns in its own table - one for each property in the address class.
    /// </remarks>
    [Owned]
    public class Address
    {

        [Required]
        [RegularExpression()]
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

        public override string ToString()
        {
            return $"{StreetName} {StreetNumber} {CityName} {PostalCode}";
        }
    }
}