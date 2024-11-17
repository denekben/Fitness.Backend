using Fitness.Application.ApplicationServices;
using Fitness.Application.Shedules.Commands;
using Fitness.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.TimeIntervals.Commands.Handlers
{
    internal sealed class DeleteTimeIntervalHandler : IRequestHandler<DeleteTimeInterval>
    {
        private readonly ITimeIntervalRepository _timeIntervalRepository;
        private readonly ILogger<DeleteTimeIntervalHandler> _logger;

        public DeleteTimeIntervalHandler(ITimeIntervalRepository timeIntervalRepository, ILogger<DeleteTimeIntervalHandler> logger)
        {
            _timeIntervalRepository = timeIntervalRepository;
            _logger = logger;
        }

        public async Task Handle(DeleteTimeInterval command, CancellationToken cancellationToken)
        {
            await _timeIntervalRepository.DeleteAsync(command.Id);

            _logger.LogInformation("TimeInterval deleted");
        }
    }
}
