using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Api.DataAccessLayer.Models
{
    /// <summary>
    /// Our customer object
    /// </summary>
    public class Customer : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        public virtual List<CustomerRides> CustomerRides { get; set; }
    }
}