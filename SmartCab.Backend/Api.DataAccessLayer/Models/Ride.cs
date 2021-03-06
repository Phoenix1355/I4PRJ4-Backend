﻿using System;
using System.ComponentModel.DataAnnotations;
using Api.DataAccessLayer.Statuses;

namespace Api.DataAccessLayer.Models
{
    /// <summary>
    /// A base ride containing all common ride attributes. 
    /// </summary>
    public class Ride
    {
        public int Id { get; set; }

        public virtual Address StartDestination { get; set; }
    
        public virtual Address EndDestination { get; set; }
        public virtual Customer Customer { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

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

        public string DeviceId { get; set; }
    }
}