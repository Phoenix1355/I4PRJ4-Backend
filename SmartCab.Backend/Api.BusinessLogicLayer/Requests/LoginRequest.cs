using System.ComponentModel.DataAnnotations;

namespace Api.BusinessLogicLayer.Requests
{
    public class LoginRequest
    {
        [Required]
        [RegularExpression(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$")]
        public string Email { get; set; }

        //Requirements for the password are set using the Identity Framework. Look in Startup.cs and see how its done
        [Required]
        public string Password { get; set; }
    }
}
