using MediatR;

namespace Fitness.Application.Reports.Commands
{
    public sealed record DeleteReport(int Id) : IRequest;
}
