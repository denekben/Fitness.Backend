using MediatR;

namespace Fitness.Application.Shedules.Commands
{
    public sealed record UpdateTimeInterval(int Id, DateTime Start, DateTime End) : IRequest;
}
