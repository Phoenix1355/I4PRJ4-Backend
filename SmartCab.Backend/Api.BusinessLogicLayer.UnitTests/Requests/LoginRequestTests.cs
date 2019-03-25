using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Api.BusinessLogicLayer.Requests;
using NUnit.Framework;

namespace Api.BusinessLogicLayer.UnitTests.Requests
{
    [TestFixture]
    public class LoginRequestTests
    {
        private const string ValidEmail = "abc@gmail.com";
        private const string ValidPassword = "Qwer111!";

        //Source for most test cases: https://blogs.msdn.microsoft.com/testing123/2009/02/06/email-address-test-cases/
        [TestCase("a.gmail.com", 1)]
        [TestCase("plaintext", 1)]
        [TestCase("#@%^%#$@#$@#.com", 1)]
        [TestCase("Michael Moeller <email@domain.com>", 1)]
        [TestCase("email.domain.com", 1)]
        [TestCase("email@domain@domain.com", 1)]
        [TestCase(".email@domain.com", 1)]
        [TestCase("email.@domain.com", 1)]
        [TestCase("email..email@domain.com", 1)]
        [TestCase("あいうえお@domain.com", 1)]
        [TestCase("email@domain.com (Joe Smith)", 1)]
        [TestCase("email@domain", 1)]
        [TestCase("email@-domain.com", 1)]
        [TestCase("email@domain..com", 1)]
        [TestCase("", 1)]
        [TestCase("email@domain.com", 0)]
        [TestCase("firstname.lastname@domain.com", 0)]
        [TestCase("email@subdomain.domain.com", 0)]
        [TestCase("firstname+lastname@domain.com", 0)]
        [TestCase("email@123.123.123.123", 0)]
        [TestCase("email@[123.123.123.123]", 0)]
        [TestCase("\"email\"@domain.com", 0)]
        [TestCase("1234567890@domain.com", 0)]
        [TestCase("email@domain-one.com", 0)]
        [TestCase("email@domain.name", 0)]
        [TestCase("email@domain.co.jp", 0)]
        [TestCase("firstname-lastname@domain.com", 0)]
        public void Email_WhenSet_ValidatesInput(string email, int numberOfErrors)
        {
            var request = new LoginRequest
            {
                Email = email,
                Password = ValidPassword,
            };

            Assert.That(ValidateModel(request).Count, Is.EqualTo(numberOfErrors));
        }

        //Remember: The data annotations set on the password property is only checking if it is set.
        //Identity framework is used to define requirements for the actual password. This is not tested.
        [TestCase(null, 1)]
        [TestCase("", 1)]
        [TestCase("a", 0)]
        public void Password_WhenSet_ValidatesInput(string password, int numberOfErrors)
        {
            var request = new LoginRequest
            {
                Email = ValidEmail,
                Password = password,
            };

            Assert.That(ValidateModel(request).Count, Is.EqualTo(numberOfErrors));
        }

        /// <summary>
        /// Source: https://stackoverflow.com/questions/2167811/unit-testing-asp-net-dataannotations-validation
        /// Validates an object based on the data annotations set on the object's properties and returns the validation result.
        /// </summary>
        /// <param name="model">The object to validate</param>
        /// <returns>The validation result for the object.</returns>
        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}