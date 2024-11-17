using Fitness.Shared.DTOs;
using MediatR;

namespace Fitness.Application.Identity.Commands
{
    public sealed record RegisterNewUser(
        string FirstName, 
        string LastName, 
        string? Phone, 
        string Password, 
        string Email, 
        DateTime? DateOfBirth, 
        string? Sex) : IRequest<TokensDto>;
}
