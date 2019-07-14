using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Api.DataAccessLayer.UnitTests.Fakes
{
    public class FakeUserManager : UserManager<IdentityUser>
    {
        public FakeUserManager()
            : base( Substitute.For<IUserStore<IdentityUser>>(),
                Substitute.For<IOptions<IdentityOptions>>(),
                Substitute.For<IPasswordHasher<IdentityUser>>(),
                new IUserValidator<IdentityUser>[0],
                new IPasswordValidator<IdentityUser>[0],
                Substitute.For<ILookupNormalizer>(),
                Substitute.For<IdentityErrorDescriber>(),
                Substitute.For<IServiceProvider>(),
                Substitute.For<ILogger<UserManager<IdentityUser>>>())
        { }

        public IdentityResult CreateAsyncReturn { get; set; } = IdentityResult.Success;

        public IdentityResult AddToRoleAsyncReturn { get; set; } = IdentityResult.Success;
        
        public IdentityResult ChangeEmailAsyncReturn { get; set; } = IdentityResult.Success;

        public IdentityResult ChangePasswordAsyncReturn { get; set; } = IdentityResult.Success;

        public string GenerateChangeEmailTokenAsyncReturn { get; set; } = "Some token";

        public override Task<string> GenerateChangeEmailTokenAsync(IdentityUser user, string email)
        {
            return Task.FromResult(GenerateChangeEmailTokenAsyncReturn);
        }

        public override Task<IdentityResult> CreateAsync(IdentityUser user, string password)
        {
            return Task.FromResult(CreateAsyncReturn);
        }

        public override Task<IdentityResult> AddToRoleAsync(IdentityUser user, string role)
        {
            return Task.FromResult(AddToRoleAsyncReturn);
        }

        public override Task<IdentityResult> ChangeEmailAsync(IdentityUser user, string email, string token)
        {
            return Task.FromResult(ChangeEmailAsyncReturn);
        }

        public override Task<IdentityResult> ChangePasswordAsync(IdentityUser user, string password, string oldPassword)
        {
            return Task.FromResult(ChangePasswordAsyncReturn);
        }
    }
}