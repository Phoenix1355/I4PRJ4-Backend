using System.ComponentModel.DataAnnotations;

namespace Api.ViewModels
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}