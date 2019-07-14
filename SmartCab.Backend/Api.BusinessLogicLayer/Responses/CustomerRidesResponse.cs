using System.Collections.Generic;
using Api.BusinessLogicLayer.DataTransferObjects;

namespace Api.Responses
{
    public class CustomerRidesResponse
    {
        public List<RideDto> Rides { get; set; }
    }
}