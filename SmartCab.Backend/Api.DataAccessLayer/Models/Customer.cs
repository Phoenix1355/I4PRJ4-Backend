using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Api.DataAccessLayer.Models
{
    /// <summary>
    /// This class represents a customer account.
    /// </summary>
    /// <remarks>
    /// Inherits from IdentityUser which means a customer object has access to all the properties in the identity framework.<br/>
    /// </remarks>
    public class Customer : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        public virtual List<Ride> Rides { get; set; }
    }
}