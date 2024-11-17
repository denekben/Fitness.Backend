using MediatR;

namespace Fitness.Application.Requests.Commands
{
    public sealed record AcceptRequest(int Id) : IRequest;

}
