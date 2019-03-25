using System;
using System.Linq;
using System.Threading.Tasks;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.Repositories
{
    public class CustomerRepository : ICustomerRepository, IDisposable
    {
        private readonly ApplicationContext _context;
        private readonly ApplicationUserRepository _applicationUserRepository;
        public CustomerRepository(ApplicationContext context, ApplicationUserRepository applicationUserRepository)
        {
            _context = context;
            _applicationUserRepository = applicationUserRepository;
        }

        public async Task<Customer> AddCustomerAsync(ApplicationUser user, Customer customer, string password)
        {

            var result = await _applicationUserRepository.AddApplicationUserAsync(user, password);

            var x = await _applicationUserRepository.AddToRoleAsync(user, nameof(Customer));
            if (result.Succeeded && x.Succeeded)
            {
                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();
                return customer;
            }
            throw new ArgumentException();
            //_applicationUserRepository.deleteA
            //tho
            //Throw identity result fault. 
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