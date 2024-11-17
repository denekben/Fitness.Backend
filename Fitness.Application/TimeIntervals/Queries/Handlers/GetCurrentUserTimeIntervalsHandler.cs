using Fitness.Application.ApplicationServices;
using Fitness.Domain.Repositories;
using Fitness.Shared.DTOs;
using Fitness.Shared.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.TimeIntervals.Queries.Handlers
{
    internal sealed class GetCurrentUserTimeIntervalsHandler : IRequestHandler<GetCurrentUserTimeIntervals, List<TimeIntervalDto>?>
    {
        private readonly ITimeIntervalRepository _timeIntervalRepository;
        private readonly ILogger<GetCurrentUserTimeIntervalsHandler> _logger;
        private readonly IHttpContextService _contextService;

        public GetCurrentUserTimeIntervalsHandler(ITimeIntervalRepository timeIntervalRepository, ILogger<GetCurrentUserTimeIntervalsHandler> logger, 
            IHttpContextService contextService)
        {
            _timeIntervalRepository = timeIntervalRepository;
            _logger = logger;
            _contextService = contextService;
        }

        public async Task<List<TimeIntervalDto>?> Handle(GetCurrentUserTimeIntervals query, CancellationToken cancellationToken)
        {
            var userId = _contextService.GetCurrentUserId();

            var intervals = await _timeIntervalRepository.GetAsync(userId, query.PageNumber, query.PageSize, query.IsDescending);

            _logger.LogInformation("Intervals were found");

            return intervals;
        }
    }
}
