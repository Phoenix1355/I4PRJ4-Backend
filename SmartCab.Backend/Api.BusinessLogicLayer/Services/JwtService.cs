using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
using Api.DataAccessLayer;
using Api.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.BusinessLogicLayer.Services
{
    /// <summary>
    /// Exposes methods containing business logic related to the generation of Json Web Tokens.
    /// </summary>
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor for this class.
        /// </summary>
        /// <param name="configuration">Used to access the configuration file in the Api project</param>
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generates a Json Web Tokens and returns it.
        /// </summary>
        /// <param name="email">Emails for the account that the token should be issued to.</param>
        /// <param name="role">The role the token should apply to</param>
        /// <returns>A token that is tied to the specified user.</returns>
        public string GenerateJwtToken(string email, string role)
        {
            //Source: https://medium.com/@ozgurgul/asp-net-core-2-0-webapi-jwt-authentication-with-identity-mysql-3698eeba6ff8
            //Generate key used to verify the token when using it later on
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Set the expiration datetime 
            var expirationDate = DateTime.Now.AddSeconds(Convert.ToDouble(_configuration["JwtExpireSeconds"])); //TODO: Change from seconds to eg. days

            //Set claims for the token. These can be accessed later on
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, email));
            claims.Add(new Claim(ClaimTypes.Role, role));
            claims.Add(new Claim(ClaimTypes.Expiration, expirationDate.ToString(CultureInfo.InvariantCulture)));

            //Create the token, serialize and return it
            var token = new JwtSecurityToken(
                null, //we are not using this feature
                null, //we are not using this feature
                claims,
                expires: expirationDate,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
