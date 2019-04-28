using System.Collections.Generic;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Statuses;

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