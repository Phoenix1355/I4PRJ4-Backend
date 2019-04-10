using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Api.BusinessLogicLayer.UnitTests.Requests
{
    public static class ValidateModelHelper
    {
        /// <summary>
        /// Source: https://stackoverflow.com/questions/2167811/unit-testing-asp-net-dataannotations-validation
        /// Source2: https://github.com/ovation22/DataAnnotationsValidatorRecursive
        /// Validates an object based on the data annotations set on the object's properties and returns the validation result.
        /// </summary>
        /// <param name="model">The object to validate</param>
        /// <returns>The validation result for the object.</returns>
        public static IList<ValidationResult> ValidateModel(object model)
        {

            var validator = new DataAnnotationsValidator.DataAnnotationsValidator();
            var validationResults = new List<ValidationResult>();

            validator.TryValidateObjectRecursive(model, validationResults);


            return validationResults;
        }
    }
}