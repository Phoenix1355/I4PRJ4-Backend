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
    public class TaxiCompanyRepository : ITaxiCompanyRepository
    {
        private readonly IUoW _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxiCompanyRepository"/> class.
        /// </summary>
        /// <param name="context">The _context - Autoinjected </param>
        public TaxiCompanyRepository(IUoW unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

    }
}
