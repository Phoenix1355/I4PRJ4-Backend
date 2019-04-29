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
    public class UoW : IUoW
    {
        public IGenericRepository<Customer> GenericCustomerRepository { get; }
        public IGenericRepository<TaxiCompany> GenericTaxiCompanyRepository { get; }
        public IGenericRepository<Ride> GenericRideRepository { get; }
        public IGenericRepository<Order> GenericOrderRepository { get; }
        private ApplicationContext _context;
        public IIdentityUserRepository IdentityUserRepository { get; }

        public UoW(ApplicationContext context, IIdentityUserRepository identityUserRepository)
        {
            _context = context;
            GenericCustomerRepository = new GenericRepository<Customer>(_context);
            GenericRideRepository = new GenericRepository<Ride>(_context);
            GenericOrderRepository = new GenericRepository<Order>(_context);
            GenericTaxiCompanyRepository = new GenericRepository<TaxiCompany>(_context);
            IdentityUserRepository = identityUserRepository;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

    }
}
