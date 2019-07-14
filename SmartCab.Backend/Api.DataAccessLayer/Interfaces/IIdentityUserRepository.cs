using System;
using System.Threading.Tasks;
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
        Task TransactionWrapper(Func<Task> func);
        Task<IdentityResult> ChangePasswordAsync(IdentityUser user, string newPassword, string oldPassword);
        Task<IdentityResult> ChangeEmailAsync(IdentityUser user, string email);
    }
}