using MediatR;

namespace Fitness.Application.Requests.Commands
{
    public sealed record CreateRequest(
        DateTime? DateTime, string? Subject, int CoachId, int TariffId
        ) : IRequest;
}
