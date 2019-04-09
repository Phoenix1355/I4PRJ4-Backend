using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using Api.DataAccessLayer.Models;

namespace Api.BusinessLogicLayer.CustomDataAnnotations
{
    //Inspiration and taken from https://stackoverflow.com/questions/11757013/regular-expressions-for-city-name
    public class GreaterThanCurrentDateTime : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var dateTime = (DateTime)value;

            if (dateTime >= DateTime.Now)
            {
                return true;
            }

            return false;
        }
    }
}