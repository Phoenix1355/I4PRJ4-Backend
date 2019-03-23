﻿using System.ComponentModel.DataAnnotations;

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
        //Regex taken from: https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
        [Required]
        [RegularExpression(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$")]
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