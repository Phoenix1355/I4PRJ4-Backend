using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.DataAccessLayer.Models
{
    public class TaxiCompany
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual List<CustomerRides> CustomerRides { get; set; }
    }
}
