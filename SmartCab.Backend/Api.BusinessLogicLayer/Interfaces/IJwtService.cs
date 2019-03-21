using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Api.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Defines a number of methods used to generate Json Web Tokens
    /// </summary>
    public interface IJwtService
    {
        string GenerateJwtToken(string email, IdentityUser user, string role);
    }
}