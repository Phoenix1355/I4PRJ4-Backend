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

        public async Task<Customer> EditCustomerAsync(Customer newCustomer, string customerId, string password, string oldPassword)
        {
            return newCustomer;
            /*
            using (var transaction = _context.Database.BeginTransaction())
            {
                var customer = await _context.Customers.FindAsync(customerId);

                if (newCustomer.Name != customer.Name)
                    customer.Name = newCustomer.Name;

                if (newCustomer.PhoneNumber != customer.PhoneNumber)
                    customer.PhoneNumber = newCustomer.PhoneNumber;

                var identityResult = await UnitOfWork.IdentityUserRepository.EditIdentityUserAsync(customer, newCustomer, password, oldPassword);

                if (identityResult.Succeeded)
                {
                    _context.Customers.Update(customer);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return customer;
                }
                transaction.Rollback();

                var error = identityResult.Errors.FirstOrDefault()?.Description;
                throw new IdentityException(error);
            }*/
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