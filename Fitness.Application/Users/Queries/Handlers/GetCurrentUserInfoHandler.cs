using Fitness.Domain.Repositories;
using Fitness.Shared.DTOs;
using Fitness.Shared.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.Users.Queries.Handlers
{
    internal class GetCurrentUserInfoHandler : IRequestHandler<GetCurrentUserInfo, ProfileDto?>
    {
        private readonly IHttpContextService _contextService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetCurrentUserInfoHandler> _logger;

        public GetCurrentUserInfoHandler(IHttpContextService contextService, IUserRepository userRepository, ILogger<GetCurrentUserInfoHandler> logger)
        {
            _contextService = contextService;
            _userRepository = userRepository;
            _logger = logger;
        }


        public async Task<ProfileDto?> Handle(GetCurrentUserInfo query, CancellationToken cancellationToken)
        {
            var userId = _contextService.GetCurrentUserId();

            var user = await _userRepository.GetUserAsync(userId);

            _logger.LogInformation($"User found {user?.Id}");

            return user;

        }
    }
}
