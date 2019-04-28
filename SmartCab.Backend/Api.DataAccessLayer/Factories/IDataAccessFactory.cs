using System;
using System.Collections.Generic;
using System.Text;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.UnitOfWork;

namespace Api.DataAccessLayer.Factories
{
    public interface IDataAccessFactory
    {
        IUoW UnitOfWork { get; }

        ICustomerRepository CustomerRepository { get; }
        IOrderRepository OrderRepository { get; }
        IRideRepository RideRepository { get; }
        ITaxiCompanyRepository TaxiCompanyRepository { get; }
        IIdentityUserRepository IdentityUserRepository { get; }
    }
}
