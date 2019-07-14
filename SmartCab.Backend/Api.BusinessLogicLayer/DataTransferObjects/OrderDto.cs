using System.Collections.Generic;

namespace Api.BusinessLogicLayer.DataTransferObjects
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public List<RideDto> Rides { get; set; }
    }
}