using System;
using System.ComponentModel.DataAnnotations;

namespace Api.BusinessLogicLayer.CustomDataAnnotations
{
    //Inspiration and taken from https://stackoverflow.com/questions/11757013/regular-expressions-for-city-name
    public class GreaterThanCurrentDateTimeValidation : ValidationAttribute
    {
        /// <summary>
        /// Validates that the given datetime is in the future
        /// </summary>
        /// <param name="value">Datetime object</param>
        /// <returns></returns>
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