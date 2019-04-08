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
    /// TaxiCompanyRepository with autoinjection of context and identityUserRepository. 
    /// </summary>
    /// <seealso cref="Api.DataAccessLayer.Interfaces.ITaxiCompanyRepository" />
    /// <seealso cref="System.IDisposable" />
    public class TaxiCompanyRepository : ITaxiCompanyRepository, IDisposable
    {
        private readonly ApplicationContext _context;
        private readonly IIdentityUserRepository _identityUserRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxiCompanyRepository"/> class.
        /// </summary>
        /// <param name="context">The context - Autoinjected</param>
        /// <param name="identityUserRepository">The application user repository - Autoinjected</param>
        public TaxiCompanyRepository(ApplicationContext context, IIdentityUserRepository identityUserRepository)
        {
            _context = context;
            _identityUserRepository = identityUserRepository;
        }

        /*
        /// <summary>
        /// Adds the customer asynchronous in a transaction
        /// </summary>
        /// <param name="taxicompany">The customer to add</param>
        /// <param name="password">The users password </param>
        /// <returns></returns>
        /// <exception cref="IdentityException"></exception>
        public async Task<TaxiCompany> AddTaxiCompanyAsync(TaxiCompany taxiCompany, string password)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var identityResult = await _identityUserRepository.AddIdentityUserAsync(taxiCompany, password);
                if (identityResult.Succeeded)
                {
                    string role = nameof(TaxiCompany);
                    var resultAddRole = await _identityUserRepository.AddToRoleAsync(taxiCompany, role);
                    if (resultAddRole.Succeeded)
                    {
                        transaction.Commit();
                        return taxiCompany;
                    }
                }
                transaction.Rollback();

                var error = identityResult.Errors.FirstOrDefault()?.Description;
                throw new IdentityException(error);
            }
            */

        /// <summary>
        /// Gets the customer asynchronous based on the email. Throws if customer doesn't exist. 
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        /// <exception cref="UserIdInvalidException">Customer does not exist.</exception>
        public async Task<TaxiCompany> GetCustomerAsync(string email)
        {
            var taxiCompany = await _context.TaxiCompanies.FirstOrDefaultAsync(c => c.Email == email);

            if (taxiCompany == null)
            {
                throw new UserIdInvalidException("TaxiCompany does not exist.");
            }

            return taxiCompany;
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
