using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;

namespace Api.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Interface for TaxiCompanyRepository
    /// </summary>
    public interface ITaxiCompanyRepository : IGenericRepository<TaxiCompany>
    {
        Task<TaxiCompany> FindByEmail(string email);
    }
}