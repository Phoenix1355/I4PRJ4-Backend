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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.Repositories
{
    /// <summary>
    /// CustomerRepository with autoinjection of _context and identityUserRepository. 
    /// </summary>
    /// <seealso cref="Api.DataAccessLayer.Interfaces.ICustomerRepository" />
    /// <seealso cref="System.IDisposable" />
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        /// <summary>
        /// Constructor for this class.
        /// </summary>
        /// <param name="context">The context used to access the database.</param>
        public CustomerRepository(ApplicationContext context) : base(context)
        {

        }

        /// <summary>
        /// Deposits the given amount to a customers account.
        /// </summary>
        /// <param name="customerId">The customers id.</param>
        /// /// <param name="deposit">The amount to deposit.</param>
        /// <returns></returns>
        /// <exception cref="NegativeDepositException">Cannot deposit negative amount.</exception>
        public async Task DepositAsync(string customerId, decimal deposit)
        {
            if (deposit <= 0)
            {
                throw new NegativeDepositException("Cannot deposit negative amount");
            }

            var customer = await FindByIDAsync(customerId);

            //Update customer
            customer.Balance += deposit;
            Update(customer);
        }

        /// <summary>
        /// Debits the given amount to a customers account.
        /// </summary>
        /// <param name="customerId">The customers id.</param>
        /// /// <param name="debit">The amount to debit.</param>
        /// <returns></returns>
        /// <exception cref="NegativeDepositException">Cannot debit negative amount.</exception>
        public async Task DebitAsync(string customerId, decimal debit)
        {
            if (debit <= 0)
            {
                throw new NegativeDepositException("Cannot debit negative amount");
            }

            var customer = await FindByIDAsync(customerId);

            //Constraints shuold be that reserved amount should atleast be debit amount, as we can never owe money to customer, 
            //Balance should atleast be debit, as a customer cannot be lend money. 
            //This in principal means that the order is invalid in some sense?
            if (customer.Balance < debit || customer.ReservedAmount < debit)
            {
                throw new InsufficientFundsException(
                    "Cannot debit order as customer does not hold enough funds. Invalid Order");
            }

            //Debit from the customer balance
            customer.Balance -= debit;
            //Remove the reserved amount. 
            customer.ReservedAmount -= debit;
            Update(customer);
        }

        /// <summary>
        /// Reserves an amount equal to the price
        /// </summary>
        /// <remarks>
        /// Only succeeds if the customers has enough funds available on his/her account.<br/>
        /// Throws an InsufficientFundsException when failing to reserve the money.
        /// </remarks>
        /// <param name="customerId"></param>
        /// <param name="price"></param>
        /// <exception cref="InsufficientFundsException">Not enough credit</exception>
        public async Task ReservePriceFromCustomerAsync(string customerId, decimal price)
        {
            var customer = await FindByIDAsync(customerId);

            if ((customer.Balance - customer.ReservedAmount) >= price)
            {
                customer.ReservedAmount += price;
            }
            else
            {
                throw new InsufficientFundsException("Not enough credit");
            }

            Update(customer);
        }

        /// <summary>
        /// FindAsync customer by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<Customer> FindByEmailAsync(string email)
        {
            return FindOnlyOneAsync(customer => customer.Email == email);
        }

        /// <summary>
        /// FindAsync the customers rides
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<List<Ride>> FindCustomerRidesAsync(string customerId)
        {
            var customer = await FindByIDAsync(customerId);
            return customer.Rides;
        }
    }
}