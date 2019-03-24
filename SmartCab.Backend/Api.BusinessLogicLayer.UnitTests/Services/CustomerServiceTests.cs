using System;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Services;
using Api.DataAccessLayer.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
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
        //[Test]
        //public void AddCustomerAsync_AddingCustomerSucceeds_ReturnsAResisterResponse()
        //{
        //    var identityResult = IdentityResult.Success;
        //    _applicationUserRepository.AddApplicationUserAsync(null, null).ReturnsForAnyArgs(identityResult);

        //    Assert.That(() => _customerService.AddCustomerAsync(_request), Throws.TypeOf<ArgumentException>());
        //}
    }
}
