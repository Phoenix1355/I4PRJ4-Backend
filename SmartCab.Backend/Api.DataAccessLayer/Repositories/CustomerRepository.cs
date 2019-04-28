using System;
using System.Collections.Generic;
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
    /// GenericCustomerRepository with autoinjection of _context and identityUserRepository. 
    /// </summary>
    /// <seealso cref="Api.DataAccessLayer.Interfaces.ICustomerRepository" />
    /// <seealso cref="System.IDisposable" />
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IUoW _unitOfWork;
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerRepository"/> class.
        /// </summary>
        /// <param name="context">The _context - Autoinjected</param>
        /// <param name="identityUserRepository">The application user repository - Autoinjected</param>
        public CustomerRepository(IUoW unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Deposit amount to customer
        /// </summary>
        /// <param name="customerId">The customers id</param>
        /// /// <param name="deposit">The amount to deposit</param>
        /// <returns></returns>
        /// <exception cref="UserIdInvalidException">Customer does not exist.</exception>
        public async Task DepositAsync(string customerId, decimal deposit)
        {
            if (deposit <=0)
            {
                throw new NegativeDepositException("Cannot deposit negative amount");
            }

            var customer = _unitOfWork.GenericCustomerRepository.FindOnlyOne(c => c.Id == customerId);
            
            //Update customer
            customer.Balance += deposit;
            _unitOfWork.GenericCustomerRepository.Update(customer);
            _unitOfWork.SaveChanges();
        }


        #region IDisposable implementation



        #endregion
    }
}