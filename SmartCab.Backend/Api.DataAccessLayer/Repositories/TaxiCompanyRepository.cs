using System;
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
        /// Constructor for Taxicompany repository. 
        /// </summary>
        /// <param name="context"></param>
        public TaxiCompanyRepository(ApplicationContext context) : base(context)
        {
        }

        /// <summary>
        /// FindAsync the taxicompany with a given email otherwise throws exceptions 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<TaxiCompany> FindByEmail(string email)
        {
            return await FindOnlyOneAsync(taxicompany => taxicompany.Email == email);
        }
    }
}
