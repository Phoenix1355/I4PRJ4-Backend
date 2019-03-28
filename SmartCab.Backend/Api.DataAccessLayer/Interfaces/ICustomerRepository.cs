using System.Threading.Tasks;
using Api.DataAccessLayer.Models;

namespace Api.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Interface for CustomerRepository, containing relevant methods. 
    /// </summary>
    public interface ICustomerRepository
    {
        Task<Customer> AddCustomerAsync(Customer customer, string password);
        Task<Customer> GetCustomerAsync(string email);
    }
}