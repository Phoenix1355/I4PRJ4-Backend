using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using CustomExceptions;

namespace Api.DataAccessLayer.Interfaces
{
    public interface ITaxiCompanyRepository
    {
        Task<TaxiCompany> GetTaxiCompanyAsync(string email);
    }
}