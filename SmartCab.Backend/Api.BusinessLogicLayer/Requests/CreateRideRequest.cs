using System;
using System.ComponentModel.DataAnnotations;
using Api.DataAccessLayer.Models;

namespace Api.BusinessLogicLayer.Requests
{
    public class CreateRideRequest
    {
        //[Required]
        public bool IsShared { get; set; }

        //[Required]
        public DateTime DepartureTime { get; set; }

        //[Required]
        public DateTime ConfirmationDeadline { get; set; }

        //[Required]
        public int PassengerCount { get; set; }

        //[Required]
        public Address StartDestination { get; set; }

        //[Required]
        public Address EndDestination { get; set; }
    }
}