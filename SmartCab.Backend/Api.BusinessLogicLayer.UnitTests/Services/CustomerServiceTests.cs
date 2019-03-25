using System;
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
        public void AddCustomerAsync_AddingCustomerFails_ThrowsArgumentException()
        {
            var identityResult = IdentityResult.Failed();
            _applicationUserRepository.AddApplicationUserAsync(null, null).ReturnsForAnyArgs(identityResult);

            Assert.That(() => _customerService.AddCustomerAsync(_request), Throws.TypeOf<ArgumentException>());
        }

        //TODO: Ask Mads how to do this...
        //Would just stub it out like normal. Otherwise you need to use the real implementation of the dataAccessLayer.
        //If this is needed in the future, you can use the factory to create inmemory context, and supply the relevant Content for Repositories
        [Test]
        public void AddCustomerAsync_AddingCustomerSucceeds_ReturnsAResisterResponseToken()
        {
            var identityResult = IdentityResult.Success;
            _applicationUserRepository.AddApplicationUserAsync(null, null).ReturnsForAnyArgs(identityResult);
            Customer customer = new Customer()
            {
                ApplicationUserId = "ID",
                Name = _request.Name,
                Id = 1,
                PhoneNumber = _request.PhoneNumber
            };
            _customerRepository.AddCustomerAsync(null).ReturnsForAnyArgs<Customer>(customer);
                _jwtService.GenerateJwtToken(null, null).ReturnsForAnyArgs<string>("Token");
                

                var result = _customerService.AddCustomerAsync(_request).Result;

                Assert.That(result.Token ,Is.EqualTo("Token"));
        }

        [Test]
        public void AddCustomerAsync_AddingCustomerSucceeds_ReturnsAResisterResponseCustomer()
        {
            var identityResult = IdentityResult.Success;
            _applicationUserRepository.AddApplicationUserAsync(null, null).ReturnsForAnyArgs(identityResult);
            Customer customer = new Customer()
            {
                ApplicationUserId = "ID",
                Name = _request.Name,
                Id = 1,
                PhoneNumber = _request.PhoneNumber
            };
            _customerRepository.AddCustomerAsync(null).ReturnsForAnyArgs<Customer>(customer);

            CustomerDto customerDto = new CustomerDto()
            {
                Email = _request.Email,
                Name = _request.Name,
                PhoneNumber = _request.PhoneNumber
            };

            _mapper.Map<CustomerDto>(null).ReturnsForAnyArgs(customerDto);

            var result = _customerService.AddCustomerAsync(_request).Result;

            Assert.That(result.Customer, Is.EqualTo(customerDto));

        }

        [Test]
        public void AddCustomerAsync_AddingCustomerFailss_ThrowsArgumentException()
        {
            var identityResult = IdentityResult.Failed();
            _applicationUserRepository.AddApplicationUserAsync(null, null).ReturnsForAnyArgs(identityResult);

            Assert.That(() => _customerService.AddCustomerAsync(_request), Throws.TypeOf<ArgumentException>());
        }

    }
}
