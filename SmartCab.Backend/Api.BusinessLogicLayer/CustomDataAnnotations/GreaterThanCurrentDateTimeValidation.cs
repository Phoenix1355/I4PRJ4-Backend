using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Api.DataAccessLayer.Models;

namespace Api.BusinessLogicLayer.CustomDataAnnotations
{
    //Inspiration and taken from https://stackoverflow.com/questions/11757013/regular-expressions-for-city-name
    public class GreaterThanCurrentDateTimeValidation : ValidationAttribute
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