using Microsoft.AspNetCore.Identity;

namespace Api.DataAccessLayer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual Customer Customer { get; set; }
    }
}