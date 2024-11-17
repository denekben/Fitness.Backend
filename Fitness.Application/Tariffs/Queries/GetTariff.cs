using Fitness.Shared.DTOs;
using MediatR;

namespace Fitness.Application.Tariffs.Queries
{
    public sealed record GetTariff(
        bool IsDescending, string SearchPhrase = "", int PageNumber = 1, int PageSize = 20, decimal MinPrice = 0.0m, decimal MaxPrice = decimal.MaxValue
        ) : IRequest<List<TariffDto>?>;
}
