using System.ComponentModel.DataAnnotations;

namespace Api.BusinessLogicLayer.Requests
{
    public class LoginRequest
    {
        [Required]
        [RegularExpression(Constants.EmailRegex, ErrorMessage = Constants.EmailRegexErrorMessage)]
        public string Email { get; set; }

        //Requirements for the password are set using the Identity Framework. Look in Startup.cs and see how its done
        [Required]
        public string Password { get; set; }
    }
}
