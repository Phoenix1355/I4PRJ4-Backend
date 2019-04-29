using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.Statuses;
using CustomExceptions;

namespace Api.DataAccessLayer.UnitOfWork
{
    /// <summary>
    /// Interface for Unit of work pattern. 
    /// </summary>
    public interface IUoW
    {
        ICustomerRepository CustomerRepository { get; }
        ITaxiCompanyRepository TaxiCompanyRepository { get; }
        IRideRepository RideRepository { get; }
        IOrderRepository OrderRepository { get; }
        IIdentityUserRepository IdentityUserRepository { get; }

        void SaveChanges();
    }
}
