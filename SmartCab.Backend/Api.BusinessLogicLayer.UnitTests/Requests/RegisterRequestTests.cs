using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Api.BusinessLogicLayer.Requests;
using NUnit.Framework;

namespace Api.BusinessLogicLayer.UnitTests.Requests
{
    [TestFixture]
    public class RegisterRequestTests
    {
        private const string ValidEmail = "abc@gmail.com";
        private const string ValidPassword = "Qwer111!";
        private const string ValidName = "Michael Moeller";
        private const string ValidPhoneNumber = "28515359";

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
            var request = new RegisterRequest
            {
                Email = email,
                Password = ValidPassword,
                PasswordRepeated = ValidPassword,
                Name = ValidName,
                PhoneNumber = ValidPhoneNumber
            };

            Assert.That(ValidateModel(request).Count, Is.EqualTo(numberOfErrors));
        }

        //Remember: The data annotations set on the two password attributes is only checking if the passwords are present and equal
        //Identity framework is used to define requirements for the actual password. This is not tested.
        [TestCase("", "", 2)]
        [TestCase("", "a", 2)] //two errors because the "compare" data annotation is only put on the repeated password
        [TestCase("a", "", 1)]
        [TestCase("a", "a", 0)]
        [TestCase("abc", "qwe", 1)]
        public void Password_WhenSet_ValidatesThatBothPasswordsIsPresentAndThatTheyAreEqual(string first, string second, int numberOfErrors)
        {
            var request = new RegisterRequest
            {
                Email = ValidEmail,
                Password = first,
                PasswordRepeated = second,
                Name = ValidName,
                PhoneNumber = ValidPhoneNumber
            };

            Assert.That(ValidateModel(request).Count, Is.EqualTo(numberOfErrors));
        }

        [TestCase(0, 1)]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(3, 0)]
        [TestCase(4, 0)]
        [TestCase(254, 0)]
        [TestCase(255, 0)]
        [TestCase(256, 1)]
        [TestCase(1000, 1)]
        public void Name_WhenSet_ValidatesLengthIsMin3Max255(int numberOfCharsInName, int numberOfErrors)
        {
            var name = GenerateString(numberOfCharsInName);

            var request = new RegisterRequest
            {
                Email = ValidEmail,
                Password = ValidPassword,
                PasswordRepeated = ValidPassword,
                Name = name.ToString(),
                PhoneNumber = ValidPhoneNumber
            };

            Assert.That(ValidateModel(request).Count, Is.EqualTo(numberOfErrors));
        }

        
        [TestCase("          ", 1)]
        [TestCase(null, 1)]
        public void Name_WhenSetToWhiteSpaceOrNull_ValidationFails(string name, int numberOfErrors)
        {
            var request = new RegisterRequest
            {
                Email = ValidEmail,
                Password = ValidPassword,
                PasswordRepeated = ValidPassword,
                Name = name,
                PhoneNumber = ValidPhoneNumber
            };

            Assert.That(ValidateModel(request).Count, Is.EqualTo(numberOfErrors));
        }

        [TestCase("01234567", 1)]
        [TestCase("00000000", 1)]
        [TestCase("", 1)]
        [TestCase("1111111", 1)]
        [TestCase("111111111", 1)]
        [TestCase("12345678", 0)]
        [TestCase("23242526", 0)]
        [TestCase("99999999", 0)]
        [TestCase("19181716", 0)]
        [TestCase("-12345678", 1)]
        [TestCase("asdfasdf", 1)]
        [TestCase("a", 1)]
        [TestCase("a1a2a3a4", 1)]
        public void PhoneNumber_WhenSet_ValidatesInput(string phoneNumber, int numberOfErrors)
        {
            var request = new RegisterRequest
            {
                Email = ValidEmail,
                Password = ValidPassword,
                PasswordRepeated = ValidPassword,
                Name = ValidName,
                PhoneNumber = phoneNumber
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

        /// <summary>
        /// Generates a string with the specified number of characters.
        /// </summary>
        /// <param name="numberOfCharsInString">The number of characters in the returned string</param>
        /// <returns>A string with the specified numbers of characters.</returns>
        private string GenerateString(int numberOfCharsInString)
        {
            var str = new StringBuilder();

            for (int i = 0; i < numberOfCharsInString; i++)
            {
                str.Append("A");
            }

            return str.ToString();
        }
    }
}