using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using CustomExceptions;

namespace Api.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Interface for Taxi Company
    /// </summary>
    public interface ITaxiCompanyRepository
    {
        Task<TaxiCompany> GetTaxiCompanyAsync(string email);
    }
}