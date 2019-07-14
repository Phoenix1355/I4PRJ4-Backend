using System;
using System.ComponentModel.DataAnnotations;
using Api.BusinessLogicLayer.CustomDataAnnotations;
using Api.BusinessLogicLayer.Enums;
using Api.DataAccessLayer.Models;

namespace Api.BusinessLogicLayer.Requests
{
    public class CreateRideRequest
    {
        [Required]
        public RideType RideType { get; set; }

        [Required]
        [GreaterThanCurrentDateTimeValidation(ErrorMessage = "The departure time must be greater than the current datetime.")]
        public DateTime DepartureTime { get; set; }

        [Required]
        [GreaterThanCurrentDateTimeValidation(ErrorMessage = "The confirmation time must be greater than the current datetime.")]
        public DateTime ConfirmationDeadline { get; set; }

        [Required]
        [Range(1, 4, ErrorMessage = "The passenger count must be between 1 and 4.")]
        public int PassengerCount { get; set; }

        [Required]
        public Address StartDestination { get; set; }

        [Required]
        public Address EndDestination { get; set; }
        public string DeviceId { get; set; }
    }
}