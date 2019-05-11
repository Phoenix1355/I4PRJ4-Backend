using System;
using System.ComponentModel.DataAnnotations;
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
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ ]+$")]
        public string CityName { get; set; }

        [Required]
        [Range(1000,9999)]
        public int PostalCode { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ ]+$")]
        public string StreetName { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int StreetNumber { get; set; }

        public double Lat { get; set; }
        public double Lng { get; set; }

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