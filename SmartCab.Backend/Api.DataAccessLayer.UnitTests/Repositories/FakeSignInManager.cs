using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Api.DataAccessLayer.UnitTests.Repositories
{
    public class FakeSignInManager : SignInManager<ApplicationUser>
    {
        public FakeSignInManager()
            : base(Substitute.For<FakeUserManager>(),
                Substitute.For<IHttpContextAccessor>(),
                Substitute.For<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                Substitute.For<IOptions<IdentityOptions>>(),
                Substitute.For<ILogger<SignInManager<ApplicationUser>>>(),
                Substitute.For<IAuthenticationSchemeProvider>())
        { }

        public SignInResult PasswordSignInAsyncReturn { get; set; } = SignInResult.Success;

        public override Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return Task.FromResult(PasswordSignInAsyncReturn);
        }
    }
}