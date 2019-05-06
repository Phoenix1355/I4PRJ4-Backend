using Api.BusinessLogicLayer.DataTransferObjects;

namespace Api.BusinessLogicLayer.Responses
{
    /// <summary>
    /// Returnes the price when the endpoint api/rides/price successfully returns.
    /// <remarks>
    /// </remarks>
    /// </summary>
    public class PriceResponse
    {
        public decimal Price { get; set; }
    }
}