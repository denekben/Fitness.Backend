using MediatR;

namespace Fitness.Application.Tariffs.Commands
{
    public sealed record DeleteTariff(int Id) : IRequest;
}
