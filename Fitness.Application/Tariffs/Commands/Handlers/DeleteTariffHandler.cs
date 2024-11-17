using Fitness.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.Tariffs.Commands.Handlers
{
    internal sealed class DeleteTariffHandler : IRequestHandler<DeleteTariff>
    {
        private readonly ITariffRepository _tariffRepository;
        private readonly ILogger<DeleteTariffHandler> _logger;

        public DeleteTariffHandler(ITariffRepository tariffRepository, ILogger<DeleteTariffHandler> logger)
        {
            _tariffRepository = tariffRepository;
            _logger = logger;
        }

        public async Task Handle(DeleteTariff command, CancellationToken cancellationToken)
        {
            await _tariffRepository.DeleteAsync(command.Id);
            _logger.LogInformation("Tariff deleted");
        }
    }
}
