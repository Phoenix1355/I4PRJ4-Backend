using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.DataAccessLayer.Interfaces
{
    public interface IApplicationUserRepository
    {
        Task<IdentityResult> AddApplicationUserAsync(ApplicationUser user, string password);

        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        Task<SignInResult> SignInAsync(string email, string password);
    }
}