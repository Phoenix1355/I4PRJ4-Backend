using System;
using System.ComponentModel.DataAnnotations;

namespace SmartCabPoC.DataLayer.Models
{
    /// <summary>
    /// Represents a simplified taxi ride.
    /// </summary>
    public class Ride
    {
        public int Id { get; set; }

        public DateTime DepartureTime { get; set; }
    }
}