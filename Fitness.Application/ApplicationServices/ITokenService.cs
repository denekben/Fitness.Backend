using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Fitness.Application.ApplicationServices
{
    public interface ITokenService
    {
        string GenerateAccessToken(int id, string lastname, string email, string role);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
