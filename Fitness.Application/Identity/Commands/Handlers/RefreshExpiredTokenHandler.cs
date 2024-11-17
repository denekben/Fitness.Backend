using Fitness.Application.ApplicationServices;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Fitness.Application.Identity.Commands.Handlers
{
    internal sealed class RefreshExpiredTokenHandler : IRequestHandler<RefreshExpiredToken, string>
    {
        private readonly ITokenService _tokenService;
        private readonly IAuthService _authService;
        private readonly ILogger<RefreshExpiredTokenHandler> _logger;

        public RefreshExpiredTokenHandler(IAuthService authService, ITokenService tokenService, ILogger<RefreshExpiredTokenHandler> logger)
        {
            _authService = authService;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<string> Handle(RefreshExpiredToken command, CancellationToken cancellationToken)
        {
            var email = _tokenService.GetPrincipalFromExpiredToken(command.AccessToken)
                .Claims.SingleOrDefault(x => x.Type.Equals(ClaimTypes.Email))?.Value ??
                throw new InvalidOperationException("Cannot refresh token");

            if (!await _authService.IsRefreshTokenValid(email, command.RefreshToken))
            {
                throw new InvalidOperationException("Refresh token is invalid");
            }

            var (id, _, lastName, _, _, _, role, _, _) = await _authService.GetUserDetailsByEmailAsync(email);
            string accessToken = _tokenService.GenerateAccessToken(id, lastName, email, role)
                ?? throw new InvalidOperationException("Cannot create access token");

            _logger.LogInformation($"User {id} refreshed expired token");

            return accessToken;
        }
    }
}
