using System;
using System.ComponentModel.DataAnnotations;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Statuses;

namespace Api.BusinessLogicLayer.Responses
{
    public class CreateRideResponse
    {
        public int Id { get; set; }
        public Address StartDestination { get; set; }
        public Address EndDestination { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ConfirmationDeadline { get; set; }
        public int PassengerCount { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal Price { get; set; }
        public RideStatus Status { get; set; }
    }
}