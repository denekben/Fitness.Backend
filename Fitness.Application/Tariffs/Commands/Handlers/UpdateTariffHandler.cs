using Domain.Entities;
using Fitness.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.Tariffs.Commands.Handlers
{
    internal sealed class UpdateTariffHandler : IRequestHandler<UpdateTariff>
    {
        private readonly ITariffRepository _tariffRepository;
        private readonly ILogger<UpdateTariffHandler> _logger;

        public UpdateTariffHandler(ITariffRepository tariffRepository, ILogger<UpdateTariffHandler> logger)
        {
            _tariffRepository = tariffRepository;
            _logger = logger;
        }

        public async Task Handle(UpdateTariff command, CancellationToken cancellationToken)
        {
            await _tariffRepository.UpdateAsync(new Tariff(command.Id, command.Price, command.Description));
            _logger.LogInformation("Tariff updated");
        }
    }
}
