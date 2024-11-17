using Fitness.Shared.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Fitness.Application.ApplicationServices;

namespace Fitness.Application.Identity.Commands.Handlers
{
    internal sealed class RegisterNewUserHandler : IRequestHandler<RegisterNewUser, TokensDto>
    {
        private readonly IAuthService _authService;
        private readonly ILogger<RegisterNewUserHandler> _logger;
        private readonly ITokenService _tokenService;

        public RegisterNewUserHandler(IAuthService authService, ILogger<RegisterNewUserHandler> logger, ITokenService tokenService)
        {
            _authService = authService;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task<TokensDto> Handle(RegisterNewUser command, CancellationToken cancellationToken)
        {
            var (firstName, lastName, phone, password, email, dateOfBirth, sex) = command;

            var userIdentityId = await _authService.CreateUserAsync(firstName, lastName, phone, password, email, dateOfBirth, sex, Role.Athlete);

            await _authService.AssignUserToRole(userIdentityId, Role.Athlete);

            var refreshToken = _tokenService.GenerateRefreshToken();

            await _authService.UpdateRefreshToken(email, refreshToken);
            var accessToken = _tokenService.GenerateAccessToken(userIdentityId, lastName, email, Role.Athlete)
                ?? throw new InvalidOperationException("Cannot generate access token");

            _logger.LogInformation($"User {email} registered");
            return new TokensDto(accessToken, refreshToken);
        }
    }
}
