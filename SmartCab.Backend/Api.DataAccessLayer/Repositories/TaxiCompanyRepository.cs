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
        public TaxiCompanyRepository(ApplicationContext context) : base(context)
        {
        }

        public TaxiCompany FindByEmail(string email)
        {
            return FindOnlyOne(taxicompany => taxicompany.Email == email);
        }
    }
}
