using Fitness.Application.Shedules.Queries;
using Fitness.Domain.Repositories;
using Fitness.Shared.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.TimeIntervals.Queries.Handlers
{
    internal sealed class GetTimeIntervalsHandler : IRequestHandler<GetTimeIntervals, List<TimeIntervalDto>?>
    {
        private readonly ITimeIntervalRepository _timeIntervalRepository;
        private readonly ILogger<GetTimeIntervalsHandler> _logger;

        public GetTimeIntervalsHandler(ITimeIntervalRepository timeIntervalRepository, ILogger<GetTimeIntervalsHandler> logger)
        {
            _timeIntervalRepository = timeIntervalRepository;
            _logger = logger;
        }

        public async Task<List<TimeIntervalDto>?> Handle(GetTimeIntervals query, CancellationToken cancellationToken)
        {
            var intervals = await _timeIntervalRepository.GetAsync(query.Id, query.PageNumber, query.PageSize, query.IsDescending);

            _logger.LogInformation("Intervals were found");

            return intervals;
        }
    }
}
