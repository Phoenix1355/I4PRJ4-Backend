using System.Text;
using Api.Requests;
using NUnit.Framework;

namespace Api.BusinessLogicLayer.UnitTests.Requests
{
    [TestFixture]
    public class EditCustomerRequestTests
    {
        // No tests for OldPassword as it validates through Identity
        // No tests for ChangePassword as it's a bool with no data notation

        private const string ValidEmail = "test@domain.com";
        private const string ValidName = "Test Tester";
        private const string ValidPhoneNumber = "12345678";

        [TestCase("a.gmail.com", 1)]
        [TestCase("plaintext", 1)]
        [TestCase("#@%^%#$@#$@#.com", 1)]
        [TestCase("Test Tester <email@domain.com>", 1)]
        [TestCase("email.domain.com", 1)]
        [TestCase("email@domain@domain.com", 1)]
        [TestCase(".email@domain.com", 1)]
        [TestCase("email.@domain.com", 1)]
        [TestCase("email..email@domain.com", 1)]
        [TestCase("あいうえお@domain.com", 1)]
        [TestCase("email@domain.com (Test Tester)", 1)]
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
            var request = new EditCustomerRequest
            {
                Email = email,
                Name = ValidName,
                PhoneNumber = ValidPhoneNumber,
                ChangePassword = false
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(numberOfErrors));
        }

        // Identity framework is used to define requirements for the actual password. This is not tested.
        [TestCase("", "a", 1)] 
        [TestCase("a", "", 1)]
        [TestCase("a", "a", 0)]
        [TestCase("abc", "qwe", 1)]
        public void Password_WhenSet_ValidatesThatBothPasswordsIsPresentAndThatTheyAreEqual(string first, string second, int numberOfErrors)
        {
            var request = new EditCustomerRequest
            {
                Email = ValidEmail,
                Password = first,
                RepeatedPassword = second,
                Name = ValidName,
                PhoneNumber = ValidPhoneNumber,
                ChangePassword = false
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(numberOfErrors));
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

            var request = new EditCustomerRequest
            {
                Email = ValidEmail,
                Name = name.ToString(),
                ChangePassword = false,
                PhoneNumber = ValidPhoneNumber
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(numberOfErrors));
        }

        [TestCase("          ", 1)]
        [TestCase(null, 1)]
        public void Name_WhenSetToWhiteSpaceOrNull_ValidationFails(string name, int numberOfErrors)
        {
            var request = new EditCustomerRequest
            {
                Email = ValidEmail,
                Name = name,
                PhoneNumber = ValidPhoneNumber,
                ChangePassword = false
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(numberOfErrors));
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
            var request = new EditCustomerRequest
            {
                Email = ValidEmail,
                Name = ValidName,
                ChangePassword = false,
                PhoneNumber = phoneNumber
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(numberOfErrors));
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