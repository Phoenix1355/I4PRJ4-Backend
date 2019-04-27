using System.ComponentModel.DataAnnotations;
using Api.BusinessLogicLayer;

namespace Api.Requests
{
    public class EditCustomerRequest
    {
        [StringLength(255, MinimumLength = 3,ErrorMessage = "Name must minimum 3 characters and a maximum of 255 characters.")]
        public string Name { get; set; }
        [RegularExpression(Constants.EmailRegex, ErrorMessage = Constants.EmailRegexErrorMessage)]
        public string Email { get; set; }
        [RegularExpression(Constants.PhoneNumberRegex, ErrorMessage = Constants.PhoneNumberRegexErrorMessage)] // 8 digits and cannot start with 0
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}