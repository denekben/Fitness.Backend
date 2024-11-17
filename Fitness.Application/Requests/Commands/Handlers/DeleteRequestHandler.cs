using Fitness.Application.ApplicationServices;
using Fitness.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.Requests.Commands.Handlers
{
    internal sealed class DeleteRequestHandler : IRequestHandler<DeleteRequest>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly ILogger<DeleteRequestHandler> _logger;

        public DeleteRequestHandler(IRequestRepository requestRepository, ILogger<DeleteRequestHandler> logger)
        {
            _requestRepository = requestRepository;
            _logger = logger;
        }

        public async Task Handle(DeleteRequest command, CancellationToken cancellationToken)
        {
            await _requestRepository.DeleteAsync(command.Id);

            _logger.LogInformation("Request deleted");
        }
    }
}
