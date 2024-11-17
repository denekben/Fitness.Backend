using Domain.Entities;
using Fitness.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.Tariffs.Commands.Handlers
{
    internal sealed class CreateTarrifHandler : IRequestHandler<CreateTariff>
    {
        private readonly ITariffRepository _tariffRepository;
        private readonly ILogger<CreateTarrifHandler> _logger;

        public CreateTarrifHandler(ITariffRepository tariffRepository, ILogger<CreateTarrifHandler> logger)
        {
            _tariffRepository = tariffRepository;
            _logger = logger;
        }

        public async Task Handle(CreateTariff command, CancellationToken cancellationToken)
        {
            await _tariffRepository.AddAsync(new Tariff(command.Price, command.Description));
            _logger.LogInformation("Tariff created");
        }
    }
}
