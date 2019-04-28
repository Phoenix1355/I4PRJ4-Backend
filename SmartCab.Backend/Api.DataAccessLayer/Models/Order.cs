using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Api.DataAccessLayer.Statuses;

namespace Api.DataAccessLayer.Models
{
    /// <summary>
    /// Represents an order which a taxi company can choose to accept.
    /// </summary>
    /// <remarks>
    /// An order contains one ride if the order is related to a solo ride.<br/>
    /// An order will contain two or more rides if the order is related to shared rides
    /// </remarks>
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public OrderStatus Status { get; set; }
        public virtual List<Ride> Rides { get; set; }
        [Required]
        public decimal Price { get; set; }

        public virtual TaxiCompany TaxiCompany { get; set; }
        public string TaxiCompanyId { get; set; }
    }
}