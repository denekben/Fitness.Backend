using Fitness.Shared.DTOs;
using MediatR;


namespace Fitness.Application.Users.Queries
{
    public sealed record GetCoaches(
        string? Sex,
        int PageNumber = 1, 
        int PageSize =10, 
        string SearchPhrase ="", 
        bool IsDescending = false
        ) : IRequest<List<ProfileDto>?>;
}
