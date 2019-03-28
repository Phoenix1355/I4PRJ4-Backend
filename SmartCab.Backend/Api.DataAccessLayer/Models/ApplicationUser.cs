using Api.DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Api.DataAccessLayer.Models
{
    /// <summary>
    /// IdentityUser, containing our navigation properties for Customer and TaxiCompani. 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Identity.IdentityUser" />
    public class ApplicationUser : IdentityUser
    {
        public virtual Customer Customer { get; set; }
        public virtual TaxiCompany TaxiCompany { get; set; }
    }
}