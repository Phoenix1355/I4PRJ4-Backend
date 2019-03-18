using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.BusinessLogicLayer.Interfaces
{
    public interface IIdentityService
    {
        Task<IdentityResult> AddApplicationUserAsync(ApplicationUser user, string password);

        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
    }
}