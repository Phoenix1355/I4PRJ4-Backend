using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using CustomExceptions;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.Repositories
{
    /// <summary>
    /// CustomerRepository with autoinjection of context and identityUserRepository. 
    /// </summary>
    /// <seealso cref="Api.DataAccessLayer.Interfaces.ICustomerRepository" />
    /// <seealso cref="System.IDisposable" />
    public class CustomerRepository : ICustomerRepository, IDisposable
    {
        private readonly ApplicationContext _context;
        private readonly IIdentityUserRepository _identityUserRepository;
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerRepository"/> class.
        /// </summary>
        /// <param name="context">The context - Autoinjected</param>
        /// <param name="identityUserRepository">The application user repository - Autoinjected</param>
        public CustomerRepository(ApplicationContext context, IIdentityUserRepository identityUserRepository)
        {
            _context = context;
            _identityUserRepository = identityUserRepository;
        }

        /// <summary>
        /// Adds the customer asynchronous in a transaction
        /// </summary>
        /// <param name="customer">The customer to add</param>
        /// <param name="password">The users password </param>
        /// <returns></returns>
        /// <exception cref="IdentityException"></exception>
        public async Task<Customer> AddCustomerAsync(Customer customer, string password)
        {
            using (var transaction = _context.Database.BeginTransaction())
            { 
                var identityResult = await _identityUserRepository.AddIdentityUserAsync(customer, password);
                if (identityResult.Succeeded)
                {
                    string role = nameof(Customer);
                    var resultAddRole = await _identityUserRepository.AddToRoleAsync(customer, role);
                    if (resultAddRole.Succeeded)
                    {
                        transaction.Commit();
                        return customer;
                    }
                }
                transaction.Rollback();

                var error = identityResult.Errors.FirstOrDefault()?.Description;
                throw new IdentityException(error);
            }
        }

        /// <summary>
        /// Gets the customer asynchronous based on the email. Throws if customer doesn't exist. 
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        /// <exception cref="UserIdInvalidException">Customer does not exist.</exception>
        public async Task<Customer> GetCustomerAsync(string email)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);

            if (customer == null)
            {
                throw new UserIdInvalidException("Customer does not exist.");
            }

            return customer;
        }

        /// <summary>
        /// Deposit amount to customer
        /// </summary>
        /// <param name="customerId">The customers id</param>
        /// /// <param name="deposit">The amount to deposit</param>
        /// <returns></returns>
        /// <exception cref="UserIdInvalidException">Customer does not exist.</exception>
        public async Task<Customer> DepositAsync(string customerId, int deposit)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            if (customer == null)
            {
                throw new UserIdInvalidException("Customer does not exist.");
            }
            
            //Update customer
            customer.Balance += deposit;
            await _context.SaveChangesAsync();

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