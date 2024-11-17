using Fitness.Domain.Repositories;
using Fitness.Shared.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Fitness.Application.Users.Queries.Handlers
{
    internal sealed class GetCoachesHandler : IRequestHandler<GetCoaches, List<ProfileDto>?>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetCoachesHandler> _logger;

        public GetCoachesHandler(IUserRepository userRepository, ILogger<GetCoachesHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<List<ProfileDto>?> Handle(GetCoaches query, CancellationToken cancellationToken)
        {
            var coaches = await _userRepository.GetCoachesAsync(query.PageNumber, query.PageSize, query.SearchPhrase,query.IsDescending,query.Sex);
            _logger.LogInformation("Coaches were found");
            return coaches;
        }
    }
}
