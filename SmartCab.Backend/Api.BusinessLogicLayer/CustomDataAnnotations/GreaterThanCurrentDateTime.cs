using System;
using System.ComponentModel.DataAnnotations;

namespace Api.BusinessLogicLayer.CustomDataAnnotations
{
    public class GreaterThanCurrentDateTime : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var dateTime = (DateTime) value;

            if (dateTime >= DateTime.Now)
            {
                return true;
            }

            return false;
        }
    }
}