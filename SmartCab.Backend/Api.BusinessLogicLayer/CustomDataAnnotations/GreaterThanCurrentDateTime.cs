using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using Api.DataAccessLayer.Models;

namespace Api.BusinessLogicLayer.CustomDataAnnotations
{
    //Inspiration and taken from https://stackoverflow.com/questions/11757013/regular-expressions-for-city-name
    public class AddressValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var address = (Address) value;

            //Post nummer
            if (address.PostalCode<1000 || address.PostalCode>9999)
            {
                return false;
            }

            //By
            if (Regex.IsMatch(address.CityName, @"[^a-zA-Zæøå ]"))
            {
                return false;
            }

            //Gade
            if (Regex.IsMatch(address.StreetName, @"[^a-zA-Zæøå ]"))
            {
                return false;
            }

            //Nummer
            if (address.StreetNumber < 1)
            {
                return false;
            }

            return true;
        }
    }
}