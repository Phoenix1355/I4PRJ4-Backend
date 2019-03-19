using System.ComponentModel.DataAnnotations;

namespace Api.BusinessLogicLayer.Requests
{
    /// <summary>
    /// Defines the data needed to create a new customer
    /// <remarks>
    /// When the endpoint api/customer/register is called by a client data complying with this class' data members must be supplied.
    /// <br/>
    /// Data annotations are used to validate the supplied data.
    /// </remarks>
    /// </summary>
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required] //Requirements for the password are set using the Identity Framework. Look in Startup.cs and see how its done
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "The two passwords must match.")]
        public string PasswordRepeated { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Name must be minimum 3 characters and a maximum of 255 characters.")]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^[1-9][0-9]{7}$", ErrorMessage = "The phone number must consist of exactly 8 numbers and cannot start with 0.")] //8 digits and cannot start with 0
        public string PhoneNumber { get; set; }
    }
}