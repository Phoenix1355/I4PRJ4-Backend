using System.Collections.Generic;
using Api.BusinessLogicLayer.DataTransferObjects;

namespace Api.BusinessLogicLayer.Responses
{
    public class OpenOrdersResponse
    {
        public List<OrderDto> Orders { get; set; }
    }
}