using System.Collections.Generic;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.DataAccessLayer.Models;

namespace Api.Responses
{
    public class CustomerRidesResponse
    {
        public List<RideDto> Rides { get; set; }
    }
}