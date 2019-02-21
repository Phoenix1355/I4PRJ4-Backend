using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Identity.PoC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual Customer Customer { get; set; }

        public virtual TaxiCompany TaxiCompany { get; set; }
    }
}