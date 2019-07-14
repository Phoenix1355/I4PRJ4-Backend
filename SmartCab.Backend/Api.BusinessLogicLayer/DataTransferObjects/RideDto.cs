using System;
using Api.DataAccessLayer.Models;

namespace Api.BusinessLogicLayer.DataTransferObjects
{
    public class RideDto
    {
        public string CustomerId { get; set; }
        public DateTime DepartureTime { get; set; }
        public virtual Address StartDestination { get; set; }
        public virtual Address EndDestination { get; set; }
        public DateTime ConfirmationDeadline { get; set; }
        public int PassengerCount { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
    }
}
