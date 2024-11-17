using Domain.Entities;
using Fitness.Domain.Repositories;
using Fitness.Shared.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.Requests.Commands.Handlers
{
    internal sealed class CreateRequestHandler : IRequestHandler<CreateRequest>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly ILogger<CreateRequestHandler> _logger;
        private readonly IHttpContextService _contextService;

        public CreateRequestHandler(IRequestRepository requestRepository, ILogger<CreateRequestHandler> logger, 
            IHttpContextService contextService)
        {
            _requestRepository = requestRepository;
            _logger = logger;
            _contextService = contextService;
        }

        public async Task Handle(CreateRequest command, CancellationToken cancellationToken)
        {
            var athleteId = _contextService.GetCurrentUserId();

            await _requestRepository.AddAsync(new Request(command.DateTime, command.Subject, command.CoachId, athleteId, command.TariffId));

            _logger.LogInformation("Request created");
        }
    }
}
