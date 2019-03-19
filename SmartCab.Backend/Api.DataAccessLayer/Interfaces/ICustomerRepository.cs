using System.Threading.Tasks;
using Api.DataAccessLayer.Models;

namespace Api.DataAccessLayer.Interfaces
{
    public interface ICustomerRepository
    {
        Task<ICustomer> AddCustomerAsync(ICustomer customer);
    }
}