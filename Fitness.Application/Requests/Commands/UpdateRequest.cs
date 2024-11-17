using MediatR;

namespace Fitness.Application.Requests.Commands
{
    public sealed record UpdateRequest(
        int Id, DateTime? DateTime, string? Subject, int CoachId, int TariffId
        ) : IRequest;
}
