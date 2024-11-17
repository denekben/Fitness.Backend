using Fitness.Application.ApplicationServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.Identity.Commands.Handlers
{
    internal sealed class AssignAthleteToCoachRoleHandler : IRequestHandler<AssignAthleteToCoachRole>
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AssignAthleteToCoachRoleHandler> _logger;

        public AssignAthleteToCoachRoleHandler(IAuthService authService, ILogger<AssignAthleteToCoachRoleHandler> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        public async Task Handle(AssignAthleteToCoachRole command, CancellationToken cancellationToken)
        {
            await _authService.AssignUserToRole(command.Id, command.Role);

            _logger.LogInformation($"User {command.Id} assigned to role {command.Role}");
        }
    }
}
