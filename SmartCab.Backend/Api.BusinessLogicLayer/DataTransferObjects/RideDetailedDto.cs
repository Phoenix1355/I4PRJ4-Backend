using System;
using System.Collections.Generic;
using System.Text;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Statuses;

namespace Api.BusinessLogicLayer.DataTransferObjects
{
    public class RideDetailedDto
    {
        public string CustomerId { get; set; }
        public CustomerDto Customer { get; set; }
        public DateTime DepartureTime { get; set; }
        public virtual Address StartDestination { get; set; }
        public virtual Address EndDestination { get; set; }
        public DateTime ConfirmationDeadline { get; set; }
        public int PassengerCount { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
    }
}
