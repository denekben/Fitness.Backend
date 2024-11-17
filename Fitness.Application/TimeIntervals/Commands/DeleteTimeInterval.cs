using MediatR;

namespace Fitness.Application.Shedules.Commands
{
    public sealed record DeleteTimeInterval(int Id) : IRequest;
}
