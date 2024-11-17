using Fitness.Shared.DTOs;
using MediatR;

namespace Fitness.Application.Identity.Commands
{
    public sealed record SignIn(string Email, string Password) : IRequest<TokensDto>;
}
