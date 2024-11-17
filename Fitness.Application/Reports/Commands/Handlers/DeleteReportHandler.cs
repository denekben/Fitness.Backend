using Domain.Entities;
using Fitness.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.Reports.Commands.Handlers
{
    internal sealed class DeleteReportHandler : IRequestHandler<DeleteReport>
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILogger<CreateReportHandler> _logger;

        public DeleteReportHandler(IReportRepository reportRepository, ILogger<CreateReportHandler> logger)
        {
            _reportRepository = reportRepository;
            _logger = logger;
        }

        public async Task Handle(DeleteReport command, CancellationToken cancellationToken)
        {
            await _reportRepository.DeleteAsync(command.Id);
            _logger.LogInformation("Report deleted");
        }
    }
}
