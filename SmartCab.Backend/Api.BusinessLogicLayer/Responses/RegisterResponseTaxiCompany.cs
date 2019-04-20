using Api.BusinessLogicLayer.DataTransferObjects;

namespace Api.BusinessLogicLayer.Responses
{
    /// <summary>
    /// Defines the data returned when the endpoint api/taxiCompany/register successfully returns.
    /// <remarks>
    /// The Token property holds the token that is generated when a new taxi company is created.
    /// <br/>
    /// This token can be used for subsequent requests to the endpoint.
    /// </remarks>
    /// </summary>
    public class RegisterResponseTaxiCompany
    {
        public string Token { get; set; }
        public TaxiCompanyDto TaxiCompany { get; set; }
    }
}