using System;
using System.ComponentModel.DataAnnotations;

namespace SmartCabPoC.DataLayer.Models
{
    public class Ride
    {
        public int Id { get; set; }

        public DateTime DepartureTime { get; set; }
    }
}