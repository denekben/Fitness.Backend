using MediatR;

namespace Fitness.Application.Requests.Commands
{
    public sealed record DeleteRequest(int Id) : IRequest;
}
