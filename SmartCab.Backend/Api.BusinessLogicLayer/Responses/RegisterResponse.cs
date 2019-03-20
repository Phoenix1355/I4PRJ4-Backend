using Api.BusinessLogicLayer.DataTransferObjects;

namespace Api.BusinessLogicLayer.Responses
{
    /// <summary>
    /// Defines the data returned when the endpoint api/customer/register successfully returns.
    /// <remarks>
    /// The Token property holds the token that is generated when a new customer is created.
    /// <br/>
    /// This token can be used for subsequent requests to the endpoint.
    /// </remarks>
    /// </summary>
    public class RegisterResponse
    {
        public string Token { get; set; }
        public CustomerDto Customer { get; set; }
    }
}