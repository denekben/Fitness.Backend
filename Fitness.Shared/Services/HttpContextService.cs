using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Fitness.Shared.Services
{
    public class HttpContextService : IHttpContextService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HttpContextService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public int GetCurrentUserId()
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new InvalidOperationException("Cannot find user");

            if (!int.TryParse(userIdString, out var userId))
            {
                throw new InvalidOperationException("User ID is not a valid integer");
            }

            return userId;
        }

        public string GetCurrentUserRoleName()
        {
            var roleName = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value
                ?? throw new InvalidOperationException("Cannot find user's role");

            return roleName;
        }
    }
}
