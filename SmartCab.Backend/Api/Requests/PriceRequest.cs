using Api.DataAccessLayer.Models;

namespace Api.Requests
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
