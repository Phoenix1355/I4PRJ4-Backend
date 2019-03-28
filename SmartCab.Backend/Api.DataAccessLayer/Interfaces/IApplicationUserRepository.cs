using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Interface for ApplicationUserRepository, containing relevant methods. 
    /// </summary>
    public interface IApplicationUserRepository
    {
        Task<IdentityResult> AddApplicationUserAsync(IdentityUser user, string password);

        Task<IdentityResult> AddToRoleAsync(IdentityUser user, string role);
        Task<SignInResult> SignInAsync(string email, string password);
    }
}