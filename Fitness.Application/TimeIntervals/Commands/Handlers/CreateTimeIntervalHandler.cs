using Domain.Entities;
using Fitness.Application.ApplicationServices;
using Fitness.Application.Shedules.Commands;
using Fitness.Domain.Repositories;
using Fitness.Shared.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.TimeIntervals.Commands.Handlers
{
    internal sealed class CreateTimeIntervalHandler : IRequestHandler<CreateTimeInterval>
    {
        private readonly ITimeIntervalRepository _timeIntervalRepository;
        private readonly ILogger<CreateTimeIntervalHandler> _logger;
        private readonly IHttpContextService _contextService;

        public CreateTimeIntervalHandler(ITimeIntervalRepository timeIntervalRepository, ILogger<CreateTimeIntervalHandler> logger, IHttpContextService contextService)
        {
            _timeIntervalRepository = timeIntervalRepository;
            _logger = logger;
            _contextService = contextService;
        }

        public async Task Handle(CreateTimeInterval command, CancellationToken cancellationToken)
        {
            var userId = _contextService.GetCurrentUserId();

            await _timeIntervalRepository.AddAsync(new TimeInterval(command.Start, command.End, userId));

            _logger.LogInformation("TimeInterval created");
        }
    }
}
