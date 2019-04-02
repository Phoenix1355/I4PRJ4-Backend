using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Api.DataAccessLayer.Models
{
    /// <summary>
    /// This class represents a taxi company account.
    /// </summary>
    /// <remarks>
    /// Inherits from IdentityUser which means a taxi company object has access to all the properties in the identity framework.<br/>
    /// </remarks>
    public class TaxiCompany : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        public virtual List<Order> Orders { get; set; }
    }
}
