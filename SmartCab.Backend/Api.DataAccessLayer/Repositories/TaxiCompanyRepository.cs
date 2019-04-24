﻿using System;
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
    /// TaxiCompanyRepository with autoinjection of _context and identityUserRepository. 
    /// </summary>
    /// <seealso cref="Api.DataAccessLayer.Interfaces.ITaxiCompanyRepository" />
    public class TaxiCompanyRepository : ITaxiCompanyRepository
    {
        private readonly ApplicationContext _context;
        private readonly IIdentityUserRepository _identityUserRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxiCompanyRepository"/> class.
        /// </summary>
        /// <param name="context">The _context - Autoinjected </param>
        /// <param name="identityUserRepository">The application user repository - Autoinjected</param>
        public TaxiCompanyRepository(ApplicationContext context, IIdentityUserRepository identityUserRepository)
        {
            _context = context;
            _identityUserRepository = identityUserRepository;
        }

        
        /// <summary>
        /// Gets the taxiCompany asynchronous based on the email. Throws if taxiCompany doesn't exist. 
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        /// <exception cref="UserIdInvalidException"> taxiCompany does not exist.</exception>
        public async Task<TaxiCompany> GetTaxiCompanyAsync(string email)
        {
            var taxiCompany = await _context.TaxiCompanies.FirstOrDefaultAsync(c => c.Email == email);

            if (taxiCompany == null)
            {
                throw new UserIdInvalidException("TaxiCompany does not exist.");
            }

            return taxiCompany;
        }

        /// <summary>
        /// Adds the TaxiCompany asynchronous in a transaction
        /// </summary>
        /// <param name="taxicompany">The taxicompany to add</param>
        /// <param name="password">The taxicompanys password </param>
        /// <returns></returns>
        /// <exception cref="IdentityException"></exception>
        public async Task<TaxiCompany> AddTaxiCompanyAsync(TaxiCompany taxicompany, string password)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var identityResult = await _identityUserRepository.AddIdentityUserAsync(taxicompany, password);

                if (identityResult.Succeeded)
                {
                    string role = nameof(TaxiCompany);
                    var resultAddRole = await _identityUserRepository.AddToRoleAsync(taxicompany, role);
                    if (resultAddRole.Succeeded)
                    {
                        transaction.Commit();
                        return taxicompany;
                    }
                }
                transaction.Rollback();

                var error = identityResult.Errors.FirstOrDefault()?.Description;
                throw new IdentityException(error);
            }
        }
    }
}
