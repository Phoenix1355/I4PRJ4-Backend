using System.Threading.Tasks;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.DataAccessLayer.Repositories
{

    /// <summary>
    /// Implementation of IIdentityUserRepository, all methods regarding IdentityUser from Identity framework. 
    /// </summary>
    /// <seealso cref="IIdentityUserRepository" />
    public class IdentityUserRepository : IIdentityUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityUserRepository"/> class.
        /// </summary>
        /// <param name="userManager">The user manager - Autoinjected</param>
        /// <param name="signInManager">The sign in manager - Autoinjected</param>
        public IdentityUserRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Adds the IdentityUser asynchronous.
        /// </summary>
        /// <param name="user">The user to create</param>
        /// <param name="password">The users password</param>
        /// <returns></returns>
        public async Task<IdentityResult> AddIdentityUserAsync(IdentityUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        /// <summary>
        /// Adds to role to the IdentityUser asynchronous.
        /// </summary>
        /// <param name="user">The user to add the role to</param>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public async Task<IdentityResult> AddToRoleAsync(IdentityUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        /// <summary>
        /// Signs the IdentityUser in asynchronous based on email and password.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public async Task<SignInResult> SignInAsync(string email, string password)
        {
            return await _signInManager.PasswordSignInAsync(email, password, false, false);
        }

        /// <summary>
        /// Changes the identityUsers email and phone number in asynchronous.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IdentityResult> EditIdentityUserAsync(IdentityUser user, string token)
        {
            var result1 = await _userManager.ChangeEmailAsync(user, user.Email, token);
            var result2 = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, token);

            if (result1 == IdentityResult.Success && result2 == IdentityResult.Success)
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }
        /// <summary>
        /// Changes the identityUsers password in a asynchronous.
        /// </summary>
        /// <param name="newPassword"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task ChangePassword(string newPassword, string email)
        {
            var currentUser = await _userManager.FindByEmailAsync(email);

            if (currentUser != null)
            {
                await _userManager.RemovePasswordAsync(currentUser);

                await _userManager.AddPasswordAsync(currentUser, newPassword);
            }
        }
    }
}