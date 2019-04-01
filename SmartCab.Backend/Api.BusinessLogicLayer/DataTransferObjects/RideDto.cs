using System;
using System.Collections.Generic;
using System.Text;
using Api.DataAccessLayer.Models;

namespace Api.BusinessLogicLayer.DataTransferObjects
{
    class RideDto
    {
        public DateTime DepartureTime { get; set; }
        public Address[] Address { get; set; }
        public DateTime LatestConfirmed { get; set; }
        public int CountOfPassengers { get; set; }
        public int Price { get; set; }
    }
}
