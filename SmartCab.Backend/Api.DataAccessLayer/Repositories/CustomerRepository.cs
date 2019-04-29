﻿using System;
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
    /// CustomerRepository with autoinjection of _context and identityUserRepository. 
    /// </summary>
    /// <seealso cref="Api.DataAccessLayer.Interfaces.ICustomerRepository" />
    /// <seealso cref="System.IDisposable" />
    public class CustomerRepository : GenericRepository<Customer>,ICustomerRepository
    {
        /// <summary>
        /// Constructor for customerrepository, sending application context to base constructor
        /// </summary>
        /// <param name="context"></param>
        public CustomerRepository(ApplicationContext context) : base(context)
        {
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

            var customer = FindByID(customerId);
            
            //Update customer
            customer.Balance += deposit;
            Update(customer);
        }
        /// <summary>
        /// Reserves price if customers balance is high enough to be positive. 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="price"></param>
        /// /// <exception cref="UserIdInvalidException">Not enough credit</exception>
        public void ReservePriceFromCustomer(string customerId, decimal price)
        {
            var customer = FindByID(customerId);

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
        /// Find customer by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Customer FindByEmail(string email)
        {
            return FindOnlyOne(customer => customer.Email == email);
        }

        /// <summary>
        /// Find the customers rides
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public List<Ride> FindCustomerRides(string customerId)
        {
            return FindByID(customerId).Rides;
        }
    }
}