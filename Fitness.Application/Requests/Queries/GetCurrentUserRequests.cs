using Fitness.Shared.DTOs;
using MediatR;

namespace Fitness.Application.Requests.Queries
{
    public sealed record GetCurrentUserRequests(
        bool IsDescending, string SearchPhrase = "", int PageNumber = 1, int PageSize = 20
        ) : IRequest<List<RequestDto>?>;
}