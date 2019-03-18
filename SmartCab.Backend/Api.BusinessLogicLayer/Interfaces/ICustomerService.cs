using System.Threading.Tasks;
using Api.BusinessLogicLayer.Requests;
using Api.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.BusinessLogicLayer.Interfaces
{
    public interface ICustomerService
    {
        Task<string> AddCustomerAsync(RegisterRequest request);
    }
}