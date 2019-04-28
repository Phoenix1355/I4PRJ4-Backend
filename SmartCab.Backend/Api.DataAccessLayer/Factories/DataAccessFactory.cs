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
        public IUoW UnitOfWork { get; }

        public ICustomerRepository CustomerRepository
        {
            get
            {
                if (_customerRepository == null)
                {
                    _customerRepository = new CustomerRepository(UnitOfWork);
                }
                return _customerRepository;
            }
        }

        public IOrderRepository OrderRepository
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(UnitOfWork);
                }
                return _orderRepository;
            }
        }

        public IRideRepository RideRepository
        {
            get
            {
                if (_rideRepository == null)
                {
                    _rideRepository = new RideRepository(UnitOfWork);
                }
                return _rideRepository;
            }
        }

        public ITaxiCompanyRepository TaxiCompanyRepository
        {
            get
            {
                if (_taxiCompanyRepository == null)
                {
                    _taxiCompanyRepository = new TaxiCompanyRepository(UnitOfWork);
                }
                return _taxiCompanyRepository;
            }
        }

        public IIdentityUserRepository IdentityUserRepository { get; }

        # endregion
        public DataAccessFactory(IUoW unitOfWork, IIdentityUserRepository identityUserRepository)
        {
            UnitOfWork = unitOfWork;
            IdentityUserRepository = identityUserRepository;
        }
    }
}
