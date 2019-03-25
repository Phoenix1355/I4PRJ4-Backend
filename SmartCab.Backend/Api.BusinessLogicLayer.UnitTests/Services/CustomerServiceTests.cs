﻿using System;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Services;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace Api.BusinessLogicLayer.UnitTests.Services
{
    [TestFixture]
    public class CustomerServiceTests
    {
        private IJwtService _jwtService;
        private IApplicationUserRepository _applicationUserRepository;
        private ICustomerRepository _customerRepository;
        private IMapper _mapper;
        private CustomerService _customerService;
        private RegisterRequest _request;

        [SetUp]
        public void Setup()
        {
            _jwtService = Substitute.For<IJwtService>();
            _applicationUserRepository = Substitute.For<IApplicationUserRepository>();
            _customerRepository = Substitute.For<ICustomerRepository>();
            _mapper = Substitute.For<IMapper>();
            _customerService = new CustomerService(_jwtService, _customerRepository, _applicationUserRepository, _mapper);

            _request = new RegisterRequest
            {
                Email = "m@gmail.com",
                Name = "Michael",
                Password = "Qwer111!",
                PasswordRepeated = "Qwer111!",
                PhoneNumber = "12345678"
            };
        }

        [Test]
        public async Task AddCustomerAsync_AddingCustomerSucceeds_ReturnsAResisterResponseThatContainsTheExpectedToken()
        {
            Customer customer = new Customer()
            {
                ApplicationUserId = "someAutogeneratedId",
                Name = _request.Name,
                PhoneNumber = _request.PhoneNumber
            };

            _customerRepository.AddCustomerAsync(null, null, null).ReturnsForAnyArgs(customer);
            _jwtService.GenerateJwtToken(null, null).ReturnsForAnyArgs("TheGeneratedToken");

            var response = await _customerService.AddCustomerAsync(_request);

            Assert.That(response.Token, Is.EqualTo("TheGeneratedToken"));
        }

        [Test]
        public async Task AddCustomerAsync_AddingCustomerSucceeds_ReturnsAResisterResponseThatContainsTheExpectedCustomerDto()
        {
            Customer customer = new Customer()
            {
                ApplicationUserId = "ID",
                Name = _request.Name,
                Id = 1,
                PhoneNumber = _request.PhoneNumber
            };

            _customerRepository.AddCustomerAsync(null, null, null).ReturnsForAnyArgs<Customer>(customer);

            CustomerDto customerDto = new CustomerDto()
            {
                Email = _request.Email,
                Name = _request.Name,
                PhoneNumber = _request.PhoneNumber
            };

            _mapper.Map<CustomerDto>(null).ReturnsForAnyArgs(customerDto);

            var response = await _customerService.AddCustomerAsync(_request);

            Assert.That(response.Customer, Is.EqualTo(customerDto));
        }
    }
}
