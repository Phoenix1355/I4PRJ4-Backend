using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Api.DataAccessLayer.Models
{
    /// <summary>
    /// Contains all commen properties regarding Taxi Companies. 
    /// </summary>
    public class TaxiCompany : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        public virtual List<Order> Orders { get; set; }
    }
}
