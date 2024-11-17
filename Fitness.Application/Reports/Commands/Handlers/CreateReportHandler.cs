using Domain.Entities;
using Fitness.Domain.Repositories;
using Fitness.Shared.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.Reports.Commands.Handlers
{
    internal sealed class CreateReportHandler : IRequestHandler<CreateReport>
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILogger<CreateReportHandler> _logger;
        private readonly IHttpContextService _contextService;

        public CreateReportHandler(IReportRepository reportRepository, ILogger<CreateReportHandler> logger, IHttpContextService contextService)
        {
            _reportRepository = reportRepository;
            _logger = logger;
            _contextService = contextService;
        }

        public async Task Handle(CreateReport command, CancellationToken cancellationToken)
        {
            var coachId = _contextService.GetCurrentUserId();

            await _reportRepository.AddAsync(new Report(command.Description, command.DateTime, command.TariffId, coachId, command.AthleteId), command.Exercises);
            _logger.LogInformation("Report created");
        }
    }
}
