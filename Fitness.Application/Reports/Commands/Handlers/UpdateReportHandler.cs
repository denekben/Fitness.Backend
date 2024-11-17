using Domain.Entities;
using Fitness.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.Reports.Commands.Handlers
{
    internal sealed class UpdateReportHandler : IRequestHandler<UpdateReport>
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILogger<UpdateReportHandler> _logger;

        public UpdateReportHandler(IReportRepository reportRepository, ILogger<UpdateReportHandler> logger)
        {
            _reportRepository = reportRepository;
            _logger = logger;
        }

        public async Task Handle(UpdateReport command, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{command.Id}, {command.DateTime}, {command.Description}, {command.TariffId}");
            await _reportRepository.UpdateAsync(command.Id, command.Description, command.DateTime, command.TariffId, command.Exercises);
            _logger.LogInformation("Report updated");
        }
    }
}
