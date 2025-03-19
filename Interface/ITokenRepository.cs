using Microsoft.AspNetCore.Identity;

namespace EventManagementSystem.Interface
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
