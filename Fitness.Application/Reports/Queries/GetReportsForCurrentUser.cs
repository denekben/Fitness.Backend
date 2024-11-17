using Fitness.Shared.DTOs;
using MediatR;

namespace Fitness.Application.Reports.Queries
{
    public sealed record GetReportsForCurrentUser(
        string SearchPhrase="", int PageNumber = 1, int PageSize = 20
        ) : IRequest<List<ReportDto>?>;
}
