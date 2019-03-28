using System.Threading.Tasks;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.DataAccessLayer.Repositories
{

    /// <summary>
    /// Implementaion of IApplicationUserRepository, all methods regarding ApplicationUser from Identity framework. 
    /// </summary>
    /// <seealso cref="Api.DataAccessLayer.Interfaces.IApplicationUserRepository" />
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserRepository"/> class.
        /// </summary>
        /// <param name="userManager">The user manager - Autoinjected</param>
        /// <param name="signInManager">The sign in manager - Autoinjected</param>
        public ApplicationUserRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            
            _signInManager = signInManager;
            
        }

        /// <summary>
        /// Adds the application user asynchronous.
        /// </summary>
        /// <param name="user">The user to create</param>
        /// <param name="password">The users password</param>
        /// <returns></returns>
        public async Task<IdentityResult> AddApplicationUserAsync(IdentityUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        /// <summary>
        /// Adds to role to user asynchronous.
        /// </summary>
        /// <param name="user">The user to add the role to</param>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public async Task<IdentityResult> AddToRoleAsync(IdentityUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        /// <summary>
        /// Signs the user in asynchronous based on email and password
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public async Task<SignInResult> SignInAsync(string email, string password)
        {
            return await _signInManager.PasswordSignInAsync(email, password, false, false);
        }
    }
}