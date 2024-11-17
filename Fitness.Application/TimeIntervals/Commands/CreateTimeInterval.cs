using MediatR;

namespace Fitness.Application.Shedules.Commands
{
    public sealed record CreateTimeInterval(DateTime Start, DateTime End) : IRequest;
}
