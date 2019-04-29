using System.Threading.Tasks;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;

namespace Api.DataAccessLayer.Repositories
{
    public class TestRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public TestRepository(ApplicationContext context) : base(context)
        {
        }

        public Task DepositAsync(string customerId, decimal deposit)
        {
            throw new System.NotImplementedException();
        }
    }
}
