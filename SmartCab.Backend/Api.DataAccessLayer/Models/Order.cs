using System.Collections.Generic;
using Api.DataAccessLayer.Statuses;

namespace Api.DataAccessLayer.Models
{
    public class Order
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
        public virtual List<Ride> Rides { get; set; }
        public decimal Price { get; set; }
    }
}