using Fitness.Shared.DTOs;
using MediatR;

namespace Fitness.Application.Shedules.Queries
{
    public sealed record GetTimeIntervals(
        int Id, bool IsDescending, int PageNumber = 1, int PageSize = 20
        ) : IRequest<List<TimeIntervalDto>?>;
}


