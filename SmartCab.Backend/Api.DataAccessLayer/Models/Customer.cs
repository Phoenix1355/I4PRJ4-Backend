using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.DataAccessLayer.Interfaces;

namespace Api.DataAccessLayer.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Email
        {
            get;
            set;
            //get => ApplicationUser.Email;
            //set => ApplicationUser.Email = value;
        }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual List<CustomerRides> CustomerRides { get; set; }
    }
}