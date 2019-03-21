using Api.BusinessLogicLayer.DataTransferObjects;

namespace Api.BusinessLogicLayer.Responses
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public CustomerDto Customer { get; set; }
    }
}
