using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using CustomExceptions;

namespace Api.DataAccessLayer.Interfaces
{
    public interface ITaxiCompanyRepository
    {
        Task<TaxiCompany> AddCustomerAsync(Customer customer, string password);
        Task<TaxiCompany> GetCustomerAsync(string email);
        Task DepositAsync(string customerId, decimal deposit);
    }
}