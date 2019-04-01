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
    
        public int  SlutDestinationId { get; set; }

        public virtual Address SlutDestination { get; set; }

        [Required]
        public DateTime LatestConfirmed { get; set; }
        
        [Required]
        public int CountPassengers { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int Price { get; set; }
        
        [Required]
        public RideStatus RideStatus { get; set; }
    }
}