using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Transactions;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using CustomExceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;

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
        /// This methods wraps a function inside a transaction.
        /// </summary>
        /// <remarks>
        /// /// See https://stackoverflow.com/questions/36636272/transactions-with-asp-net-identity-usermanager
        /// </remarks>
        /// <param name="insideTransactionFunction">Function to call inside a transaction</param>
        public async Task TransactionWrapper(Func<Task> insideTransactionFunction)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
               await insideTransactionFunction();
               scope.Complete();
            }
        }

        /// <summary>
        /// Adds the IdentityUser asynchronous.
        /// </summary>
        /// <param name="user">The user to create</param>
        /// <param name="password">The users password</param>
        /// <returns></returns>
        public async Task<IdentityResult> AddIdentityUserAsync(IdentityUser user, string password)
        {
            var result =  await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault()?.Description;
                throw new IdentityException(error);
            }
            return result;
        }

        /// <summary>
        /// Adds to role to the IdentityUser asynchronous.
        /// </summary>
        /// <param name="user">The user to add the role to</param>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        /// <exception cref="IdentityException"></exception>
        public async Task<IdentityResult> AddToRoleAsync(IdentityUser user, string role)
        {
            var result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault()?.Description;
                throw new IdentityException(error);
            }
            return result;
        }

        /// <summary>
        /// Signs the IdentityUser in asynchronous based on email and password.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="IdentityException">Login failed. Credentials was not found in the database.</exception>
        public async Task<SignInResult> SignInAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
            if (!result.Succeeded)
            {
                throw new IdentityException("Login failed. Credentials was not found in the database.");
            }
            return result;
        }

        /// <summary>
        /// Edits the users information
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <param name="newCustomer"></param>
        /// <param name="password"></param>
        /// <param name="oldPassword"></param>
        /// <returns></returns>
        public async Task<IdentityResult> EditIdentityUserAsync(IdentityUser user, Customer newCustomer, string password, string oldPassword)
        {
            IdentityResult resultEmail, resultPassword;

            if (newCustomer.Email != user.Email)
                resultEmail = await ChangeEmail(user, newCustomer);
            else
                resultEmail = IdentityResult.Success;

            if (password != "")
                resultPassword = await ChangePassword(password, user, oldPassword);
            else
                resultPassword = IdentityResult.Success;

            if (resultEmail == IdentityResult.Success && 
                resultPassword == IdentityResult.Success)
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        /// <summary>
        /// Changes the identityUsers password in a asynchronous.
        /// </summary>
        /// <param name="newPassword"></param>
        /// <param name="user"></param>
        /// <param name="oldPassword"></param>
        /// <returns></returns>
        private async Task<IdentityResult> ChangePassword(string newPassword, IdentityUser user, string oldPassword)
        {
            var response = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            await _userManager.UpdateAsync(user);
            return response;
        }

        /// <summary>
        /// Changes the email, normalized email, the username and the normalized username
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newCustomer"></param>
        /// <returns></returns>
        private async Task<IdentityResult> ChangeEmail(IdentityUser user, Customer newCustomer)
        {
            var emailConfirmationCode = await _userManager.GenerateChangeEmailTokenAsync(user, newCustomer.Email);
            var response = await _userManager.ChangeEmailAsync(user, newCustomer.Email, emailConfirmationCode);
            await _userManager.UpdateAsync(user);
            await _userManager.UpdateNormalizedEmailAsync(user);

            await _userManager.SetUserNameAsync(user, newCustomer.Email);
            await _userManager.UpdateNormalizedUserNameAsync(user);

            return response;
        }
    }
}