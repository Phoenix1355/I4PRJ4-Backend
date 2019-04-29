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
        public ICustomerRepository CustomerRepository { get; }
        public ITaxiCompanyRepository TaxiCompanyRepository { get; }
        public IRideRepository RideRepository { get; }
        public IOrderRepository OrderRepository { get; }
        private ApplicationContext _context;
        public IIdentityUserRepository IdentityUserRepository { get; }

        public UoW(ApplicationContext context, IIdentityUserRepository identityUserRepository)
        {
            _context = context;
            CustomerRepository = new CustomerRepository(_context);
            RideRepository = new RideRepository(_context);
            OrderRepository = new OrderRepository(_context);
            TaxiCompanyRepository = new TaxiCompanyRepository(_context);
            IdentityUserRepository = identityUserRepository;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
