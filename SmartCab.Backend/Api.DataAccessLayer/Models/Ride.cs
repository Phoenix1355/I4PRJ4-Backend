using System;

namespace Api.DataAccessLayer.Models
{
    public class Ride
    {
        public int Id { get; set; }
        public DateTime DepartureTime { get; set; }

        public Address StartDestination { get; set; }

        public Address SlutDestination { get; set; }

        public DateTime LatestConfirmed { get; set; }

        public int CountPassengers { get; set; }

        public DateTime CreatedAt { get; set; }

        public int Price { get; set; }
        
    }
}