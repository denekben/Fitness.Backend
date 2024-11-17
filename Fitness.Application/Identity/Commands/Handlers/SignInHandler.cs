using Fitness.Shared.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;
using Fitness.Application.ApplicationServices;

namespace Fitness.Application.Identity.Commands.Handlers
{
    internal sealed class SignInHandler : IRequestHandler<SignIn, TokensDto>
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<SignInHandler> _logger;

        public SignInHandler(IAuthService authService, ITokenService tokenService, ILogger<SignInHandler> logger)
        {
            _authService = authService;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<TokensDto> Handle(SignIn command, CancellationToken cancellationToken)
        {
            var (email, password) = command;

            await _authService.SigninUserAsync(email, password);
            var (id, _, lastName, _, _, _, role, _, _) = await _authService.GetUserDetailsByEmailAsync(email);
            
            var refreshToken = _tokenService.GenerateRefreshToken();

            await _authService.UpdateRefreshToken(email, refreshToken);

            string accessToken = _tokenService.GenerateAccessToken(id, lastName, email, role)
                ?? throw new InvalidOperationException("Cannot create access token");

            _logger.LogInformation($"User {email} signed in");
            return new TokensDto(accessToken, refreshToken);
        }
    }
}
