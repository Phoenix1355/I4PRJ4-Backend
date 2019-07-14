using System.Threading.Tasks;
using Api.DataAccessLayer.Interfaces;

namespace Api.DataAccessLayer.UnitOfWork
{
    /// <summary>
    /// Interface for Unit of work pattern. 
    /// </summary>
    public interface IUnitOfWork
    {
        ICustomerRepository CustomerRepository { get; }
        ITaxiCompanyRepository TaxiCompanyRepository { get; }
        IRideRepository RideRepository { get; }
        IOrderRepository OrderRepository { get; }
        IIdentityUserRepository IdentityUserRepository { get; }

        Task SaveChangesAsync();
    }
}
