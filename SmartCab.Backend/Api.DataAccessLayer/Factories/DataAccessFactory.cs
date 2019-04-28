using System;
using System.Collections.Generic;
using System.Text;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.UnitOfWork;

namespace Api.DataAccessLayer.Factories
{
    public class DataAccessFactory : IDataAccessFactory
    {
        #region Properties and backing fields

        private ICustomerRepository _customerRepository;
        private IOrderRepository _orderRepository;
        private IRideRepository _rideRepository;
        private ITaxiCompanyRepository _taxiCompanyRepository;
        private IIdentityUserRepository _identityUserRepository;
        public IUoW UnitOfWork { get; }

        public ICustomerRepository CustomerRepository
        {
            get
            {
                if (_customerRepository == null)
                {
                    _customerRepository = new CustomerRepository();
                }
                return _customerRepository;
            }
        }

        public IOrderRepository OrderRepository => _orderRepository;

        public IRideRepository RideRepository => _rideRepository;

        public ITaxiCompanyRepository TaxiCompanyRepository => _taxiCompanyRepository;

        public IIdentityUserRepository IdentityUserRepository => _identityUserRepository;

        # endregion
        public DataAccessFactory(IUoW unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}
