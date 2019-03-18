using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Api.BusinessLogicLayer.Interfaces
{
    public interface IJwtService
    {
        Task<string> GetJwtToken(string email, IdentityUser user);
    }
}