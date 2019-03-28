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
    /// <summary>
    /// CustomerRepository with autoinjection of context and applicationUserRepository. 
    /// </summary>
    /// <seealso cref="Api.DataAccessLayer.Interfaces.ICustomerRepository" />
    /// <seealso cref="System.IDisposable" />
    public class CustomerRepository : ICustomerRepository, IDisposable
    {
        private readonly ApplicationContext _context;
        private readonly IApplicationUserRepository _applicationUserRepository;
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerRepository"/> class.
        /// </summary>
        /// <param name="context">The context - Autoinjected</param>
        /// <param name="applicationUserRepository">The application user repository - Autoinjected</param>
        public CustomerRepository(ApplicationContext context, IApplicationUserRepository applicationUserRepository)
        {
            _context = context;
            _applicationUserRepository = applicationUserRepository;
        }

        /// <summary>
        /// Adds the customer and applicationUser asynchronous in a transaction
        /// </summary>
        /// <param name="customer">The customer to add</param>
        /// <param name="password">The users password </param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Customer> AddCustomerAsync(Customer customer, string password)
        {
            using (var transaction = _context.Database.BeginTransaction())
            { 
                var resultAddApplicationUser = await _applicationUserRepository.AddApplicationUserAsync(customer, password);
                if (resultAddApplicationUser.Succeeded)
                {
                    string role = nameof(Customer);
                    var resultAddRole = await _applicationUserRepository.AddToRoleAsync(customer, role);
                    if (resultAddRole.Succeeded)
                    {
                        transaction.Commit();
                        return customer;
                    }
                }
                transaction.Rollback();

                string error = "No changes applied";
                throw new ArgumentException(error);
            }
            

            
        }

        /// <summary>
        /// Gets the customer asynchronous based on the email. Throws if customer doesn't exist. 
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Customer does not exist.</exception>
        public async Task<Customer> GetCustomerAsync(string email)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);

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