using Fitness.Application.Tariffs.Commands.Handlers;
using Fitness.Domain.Repositories;
using Fitness.Shared.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.Tariffs.Queries.Handlers
{
    internal sealed class GetTariffHandler : IRequestHandler<GetTariff, List<TariffDto>?>
    {
        private readonly ITariffRepository _tariffRepository;
        private readonly ILogger<GetTariffHandler> _logger;

        public GetTariffHandler(ITariffRepository tariffRepository, ILogger<GetTariffHandler> logger)
        {
            _tariffRepository = tariffRepository;
            _logger = logger;
        }

        public async Task<List<TariffDto>?> Handle(GetTariff query, CancellationToken cancellationToken)
        {
            var tariffs = await _tariffRepository.GetAsync(query.PageNumber, query.PageSize, query.SearchPhrase, query.IsDescending, query.MinPrice, query.MaxPrice);

            _logger.LogInformation("Tariffs were found");
            return tariffs;
        }
    }
}
