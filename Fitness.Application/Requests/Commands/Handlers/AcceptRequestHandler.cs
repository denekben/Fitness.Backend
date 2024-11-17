using Fitness.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.Requests.Commands.Handlers
{
    internal sealed class AcceptRequestHandler : IRequestHandler<AcceptRequest>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly ILogger<AcceptRequestHandler> _logger;

        public AcceptRequestHandler(IRequestRepository requestRepository, ILogger<AcceptRequestHandler> logger)
        {
            _requestRepository = requestRepository;
            _logger = logger;
        }

        public async Task Handle(AcceptRequest command, CancellationToken cancellationToken)
        {
            await _requestRepository.AcceptRequest(command.Id);

            _logger.LogInformation("Request accepted");
        }
    }
}
