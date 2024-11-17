using Domain.Entities;
using Fitness.Application.ApplicationServices;
using Fitness.Domain.Repositories;
using Fitness.Shared.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.Requests.Commands.Handlers
{
    internal sealed class UpdateRequestHandler : IRequestHandler<UpdateRequest>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly ILogger<UpdateRequestHandler> _logger;
        private readonly IHttpContextService _contextService;

        public UpdateRequestHandler(IRequestRepository requestRepository, ILogger<UpdateRequestHandler> logger,
            IHttpContextService contextService)
        {
            _requestRepository = requestRepository;
            _logger = logger;
            _contextService = contextService;
        }

        public async Task Handle(UpdateRequest command, CancellationToken cancellationToken)
        {
            await _requestRepository.UpdateAsync(new Request(command.Id, command.DateTime, command.Subject, command.TariffId));

            _logger.LogInformation("Request updated");
        }
    }
}
