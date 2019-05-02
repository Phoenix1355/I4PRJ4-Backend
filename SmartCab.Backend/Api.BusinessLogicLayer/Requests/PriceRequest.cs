using System.ComponentModel.DataAnnotations;
using Api.BusinessLogicLayer.Enums;
using Api.DataAccessLayer.Models;

namespace Api.BusinessLogicLayer.Requests
{
    public class PriceRequest
    {
        [Required]
        public Address StartAddress { get; set; }
        [Required]
        public Address EndAddress { get; set; }
        [Required]
        public RideType RideType { get; set; }
    }
}
