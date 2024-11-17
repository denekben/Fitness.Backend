using MediatR;

namespace Fitness.Application.Tariffs.Commands
{
    public sealed record UpdateTariff(
        int Id, float Price, string? Description
        ) : IRequest;
}
