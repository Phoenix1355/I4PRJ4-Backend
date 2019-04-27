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
        Task<IdentityResult> EditIdentityUserAsync(IdentityUser user, string token, Customer newCustomer, string password, string oldPassword);
        Task<IdentityResult> ChangePassword(string newPassword, IdentityUser user, string oldPassword);
        Task<IdentityResult> ChangeEmail(IdentityUser user, Customer newCustomer);
    }
}