using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using CustomExceptions;

namespace Api.DataAccessLayer.Interfaces
{
    public interface ITaxiCompanyRepository
    {
        //Task<TaxiCompany> AddTaxiCompanyAsync(TaxiCompany taxiCompany, string password);
        Task<TaxiCompany> GetTaxiCompanyAsync(string email);
        Task DepositAsync(string customerId, decimal deposit);
    }
}