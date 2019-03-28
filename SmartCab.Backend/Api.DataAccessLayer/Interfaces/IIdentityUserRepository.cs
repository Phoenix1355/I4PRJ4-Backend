using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Interface for IdentityUserRepository, containing relevant methods. 
    /// </summary>
    public interface IIdentityUserRepository
    {
        Task<IdentityResult> AddIdentityUserAsync(IdentityUser user, string password);

        Task<IdentityResult> AddToRoleAsync(IdentityUser user, string role);
        Task<SignInResult> SignInAsync(string email, string password);
    }
}