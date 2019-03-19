using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Api.BusinessLogicLayer.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateJwtToken(string email, IdentityUser user);
    }
}