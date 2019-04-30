﻿using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.UnitOfWork;
using CustomExceptions;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.Repositories
{
    /// <summary>
    /// TaxiCompanyRepository with autoinjection of _context and identityUserRepository. 
    /// </summary>
    /// <seealso cref="Api.DataAccessLayer.Interfaces.ITaxiCompanyRepository" />
    public class TaxiCompanyRepository : GenericRepository<TaxiCompany>, ITaxiCompanyRepository
    {
        /// <summary>
        /// Constructor for this class. 
        /// </summary>
        /// <param name="context">The context used to access the database.</param>
        public TaxiCompanyRepository(ApplicationContext context) : base(context)
        {
        }

        /// <summary>
        /// FindAsync the TaxiCompany with a given email otherwise throws exceptions 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<TaxiCompany> FindByEmail(string email)
        {
            return await FindOnlyOneAsync(taxiCompany => taxiCompany.Email == email);
        }
    }
}
