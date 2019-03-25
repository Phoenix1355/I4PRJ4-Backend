using System.Threading.Tasks;
using Api.DataAccessLayer.Models;

namespace Api.DataAccessLayer.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> AddCustomerAsync(ApplicationUser user, Customer customer);
        Task<Customer> GetCustomerAsync(string email);
    }
}