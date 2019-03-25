using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.Repositories
{
    public class CustomerRepository : ICustomerRepository, IDisposable
    {
        private readonly ApplicationContext _context;
        private readonly IApplicationUserRepository _applicationUserRepository;
        public CustomerRepository(ApplicationContext context, IApplicationUserRepository applicationUserRepository)
        {
            _context = context;
            _applicationUserRepository = applicationUserRepository;
        }

        public async Task<Customer> AddCustomerAsync(ApplicationUser user, Customer customer, string password)
        {
            //using (var transaction = new TransactionScope())
            //{
                var result = await _applicationUserRepository.AddApplicationUserAsync(user, password);

                string role = nameof(Customer);
                var x = await _applicationUserRepository.AddToRoleAsync(user, role);
                if (result.Succeeded && x.Succeeded)
                {
                    await _context.Customers.AddAsync(customer);
                    await _context.SaveChangesAsync();
              //      transaction.Complete();
                    return customer;
                }
                //transaction.Dispose();
            //}
            throw new ArgumentException();
        }


        public async Task<Customer> GetCustomerAsync(string email)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.ApplicationUser.Email == email);

            if (customer == null)
            {
                throw new ArgumentNullException("Customer does not exist.");
            }

            return customer;
        }

        #region IDisposable implementation

        //Dispose pattern:
        //https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose#basic_pattern
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _context?.Dispose();
        }

        #endregion
    }
}