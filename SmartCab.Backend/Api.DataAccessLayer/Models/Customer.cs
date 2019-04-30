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
        public Customer()
        {
            Rides = new List<Ride>();
            Balance = 0;
            ReservedAmount = 0;
        }
        [Required]
        public string Name { get; set; }

        public virtual List<Ride> Rides { get; set; }

        public decimal Balance { get; set; }

        public decimal ReservedAmount { get; set; }
    }
}