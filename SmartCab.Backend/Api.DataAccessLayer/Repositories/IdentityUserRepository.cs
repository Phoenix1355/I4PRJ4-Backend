﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Api.DataAccessLayer.Interfaces;
using CustomExceptions;
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
        /// Changes the identityUsers password in a asynchronous.
        /// </summary>
        /// <param name="newPassword">The new password.</param>
        /// <param name="user">The user the changes should be applied to.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <returns></returns>
        public async Task<IdentityResult> ChangePasswordAsync(IdentityUser user, string newPassword, string oldPassword)
        {
            var response = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (response != IdentityResult.Success)
            {
                var error = response.Errors.FirstOrDefault()?.Description;
                throw new IdentityException(error);
            }
            return response;
        }

        /// <summary>
        /// Changes the email, normalized email, the username and the normalized username
        /// </summary>
        /// <param name="user">The user the change should be applied to.</param>
        /// <param name="email">The new email.</param>
        /// <returns></returns>
        public async Task<IdentityResult> ChangeEmailAsync(IdentityUser user, string email)
        {
            var emailConfirmationCode = await _userManager.GenerateChangeEmailTokenAsync(user, email);

            var response = await _userManager.ChangeEmailAsync(user, email, emailConfirmationCode);
            if (response != IdentityResult.Success)
            {
                var error = response.Errors.FirstOrDefault()?.Description;
                throw new IdentityException(error);
            }

            await _userManager.UpdateNormalizedEmailAsync(user);
            await _userManager.SetUserNameAsync(user, email);
            await _userManager.UpdateNormalizedUserNameAsync(user);

            return response;
        }
    }
}