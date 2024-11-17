using MediatR;

namespace Fitness.Application.Identity.Commands
{
    public sealed record AssignAthleteToCoachRole(int Id, string Role) : IRequest;
}
