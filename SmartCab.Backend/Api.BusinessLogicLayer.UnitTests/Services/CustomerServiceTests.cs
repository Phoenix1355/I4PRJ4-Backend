using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Services;
using Api.DataAccessLayer.Interfaces;
using AutoMapper;
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

        [SetUp]
        public void Setup()
        {
            _jwtService = Substitute.For<IJwtService>();
            _applicationUserRepository = Substitute.For<IApplicationUserRepository>();
            _customerRepository = Substitute.For<ICustomerRepository>();
            _mapper = Substitute.For<IMapper>();
            _customerService = new CustomerService(_jwtService, _customerRepository, _applicationUserRepository, _mapper);
        }

        [Test]
        public void AddCustomerAsync_InvalidEmail_ValidationFails()
        {
            

        }
    }
}