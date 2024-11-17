using MediatR;

namespace Fitness.Application.Tariffs.Commands
{
    public sealed record CreateTariff(
        float Price, string? Description
        ) : IRequest;
}
