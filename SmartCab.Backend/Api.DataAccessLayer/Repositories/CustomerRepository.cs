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
    public class CustomerRepository : GenericRepository<Customer>,ICustomerRepository
    {

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

            var customer = await FindByIDAsync(customerId);
            
            //Update customer
            customer.Balance += deposit;
            await UpdateAsync(customer);
        }
        /// <summary>
        /// Reserves price if customers balance is high enough to be positive. 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="price"></param>
        /// /// <exception cref="UserIdInvalidException">Not enough credit</exception>
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

            await UpdateAsync(customer);
        }

        /// <summary>
        /// FindAsync customer by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Customer> FindByEmailAsync(string email)
        {
            return await FindOnlyOneAsync(customer => customer.Email == email);
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