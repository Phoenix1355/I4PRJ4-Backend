using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace Identity.PoC.Models
{
    public class TaxiCompany
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string CompanyName { get; set; }
    }
}