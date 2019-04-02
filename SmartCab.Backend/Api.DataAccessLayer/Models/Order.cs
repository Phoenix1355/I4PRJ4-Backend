using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Api.DataAccessLayer.Statuses;

namespace Api.DataAccessLayer.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public OrderStatus Status { get; set; }
        public virtual List<Ride> Rides { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}