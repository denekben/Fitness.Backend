using MediatR;

namespace Fitness.Application.Identity.Commands
{
    public sealed record RefreshExpiredToken(string AccessToken, string RefreshToken) : IRequest<string>;
}
