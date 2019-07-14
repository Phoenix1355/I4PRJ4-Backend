using System.Threading.Tasks;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Repositories;

namespace Api.DataAccessLayer.UnitOfWork
{
    /// <summary>
    /// This class is used by the service layer to access the database.
    /// </summary>
    /// <remarks>
    /// This approach makes it possible to make several queries without saving the changes in between.<br/>
    /// </remarks>
    public class UnitOfWork : IUnitOfWork
    {
        public ICustomerRepository CustomerRepository { get; }
        public ITaxiCompanyRepository TaxiCompanyRepository { get; }
        public IRideRepository RideRepository { get; }
        public IOrderRepository OrderRepository { get; }
        private ApplicationContext _context;
        public IIdentityUserRepository IdentityUserRepository { get; }

        /// <summary>
        /// Constructor for this class.
        /// </summary>
        /// <param name="context">The context used to access the database.</param>
        /// <param name="identityUserRepository">The repository used for operations related to the Identity framework.</param>
        public UnitOfWork(ApplicationContext context, IIdentityUserRepository identityUserRepository)
        {
            _context = context;
            CustomerRepository = new CustomerRepository(_context);
            RideRepository = new RideRepository(_context);
            OrderRepository = new OrderRepository(_context);
            TaxiCompanyRepository = new TaxiCompanyRepository(_context);
            IdentityUserRepository = identityUserRepository;
        }

        /// <summary>
        /// Saves all changes made to the context.
        /// </summary>
        /// <returns></returns>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
