using Fitness.Shared.DTOs;
using MediatR;

namespace Fitness.Application.TimeIntervals.Queries
{
    public sealed record GetCurrentUserTimeIntervals(
        bool IsDescending, int PageNumber = 1, int PageSize = 20
        ) : IRequest<List<TimeIntervalDto>?>;
}
