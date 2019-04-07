using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using CustomExceptions;

namespace Api.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Interface for CustomerRepository, containing relevant methods. 
    /// </summary>
    public interface ICustomerRepository
    {
        Task<Customer> AddCustomerAsync(Customer customer, string password);
        Task<Customer> GetCustomerAsync(string email);
        Task<Customer> DepositAsync(string customerId, int deposit);
    }
}