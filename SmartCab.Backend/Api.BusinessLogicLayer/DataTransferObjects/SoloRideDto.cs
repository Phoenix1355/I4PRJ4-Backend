using System;
using System.Collections.Generic;
using Api.DataAccessLayer.Models;

namespace Api.BusinessLogicLayer.DataTransferObjects
{
    public class SoloRideDto
    {
        //public int Id { get; set; }
        public Address StartDestination { get; set; }
        public Address EndDestination { get; set; }
        public DateTime DepartureTime { get; set; }
        //public decimal Price { get; set; }
        public int PassengerCount { get; set; }
        public DateTime ConfirmationDeadLine { get; set; }
    }
}