using System.Collections.Generic;
using Api.DataAccessLayer.Interfaces;

namespace Api.DataAccessLayer.Models
{
    public class Customer : ICustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Email
        {
            get => ApplicationUser.Email;
            set => ApplicationUser.Email = value;
        }

        public string PhoneNumber { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public List<CustomerRides> CustomerRideses { get; set; }
    }
}