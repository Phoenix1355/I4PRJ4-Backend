using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Models;

namespace Api.DataTransferObjects
{
    public class PriceRequest
    {
        public Address StartAddress { get; set; }
        public Address EndAddress { get; set; }

        public PriceRequest(Address startAddress, Address endAddress)
        {
            StartAddress = startAddress;
            EndAddress = endAddress;
        }
    }
}
