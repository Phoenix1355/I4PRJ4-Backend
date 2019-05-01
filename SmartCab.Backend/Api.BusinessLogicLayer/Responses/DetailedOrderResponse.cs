using System;
using System.Collections.Generic;
using System.Text;
using Api.BusinessLogicLayer.DataTransferObjects;

namespace Api.BusinessLogicLayer.Responses
{
    public class DetailedOrderResponse : OrderDto
    {
        public CustomerDto RidesCustomer { get; set; }
    }
}
