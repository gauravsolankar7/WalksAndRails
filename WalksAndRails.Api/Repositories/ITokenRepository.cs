using Microsoft.AspNetCore.Identity;

namespace WalksAndRails.Api.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
