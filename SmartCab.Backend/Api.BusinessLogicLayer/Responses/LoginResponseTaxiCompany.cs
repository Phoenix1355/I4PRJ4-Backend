using Api.BusinessLogicLayer.DataTransferObjects;

namespace Api.BusinessLogicLayer.Responses
{
    public class LoginResponseTaxiCompany
    {
        public string Token { get; set; }
        public TaxiCompanyDto TaxiCompany { get; set; }
    }
}