using System;
using System.Collections.Generic;
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
        /// <param name="user">The identity user the token should be issued to.</param>
        /// <returns>A token that is tied to the specified user.</returns>
        public string GenerateJwtToken(string email, IdentityUser user)
        {
            var isCustomer = true; //TODO: Replace with call to database layer
            var isTaxiCompany = false; //TODO: Replace with call to database layer

            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(ClaimTypes.Name, email));

            if (isCustomer)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Customer"));
            }

            if (isTaxiCompany)
            {
                claims.Add(new Claim(ClaimTypes.Role, "TaxiCompany"));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expirationDate = DateTime.Now.AddSeconds(Convert.ToDouble(_configuration["JwtExpireDays"])); //TODO: Change from seconds to eg. days

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expirationDate,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
