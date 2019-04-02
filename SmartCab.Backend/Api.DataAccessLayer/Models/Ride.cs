using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Api.DataAccessLayer.Statuses;

namespace Api.DataAccessLayer.Models
{
    /// <summary>
    /// A base ride containing all common ride attributes. 
    /// </summary>
    public class Ride
    {
        public int Id { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        public int StartDestinationId { get; set; }

        public virtual Address StartDestination { get; set; }
    
        public int  EndDestinationId { get; set; }

        public virtual Address EndDestination { get; set; }

        [Required]
        public DateTime ConfirmationDeadline { get; set; }
        
        [Required]
        public int PassengerCount { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public RideStatus Status { get; set; }

        public virtual Customer Customer { get; set; }
    }
}