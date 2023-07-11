using Microsoft.AspNetCore.Identity;

namespace WebApiPlayground.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, string[] roles);
    }
}
