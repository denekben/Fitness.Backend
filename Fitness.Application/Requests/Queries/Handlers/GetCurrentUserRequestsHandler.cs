using Fitness.Application.ApplicationServices;
using Fitness.Domain.Repositories;
using Fitness.Shared.DTOs;
using Fitness.Shared.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.Requests.Queries.Handlers
{
    internal sealed class GetCurrentUserRequestsHandler : IRequestHandler<GetCurrentUserRequests, List<RequestDto>?>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly ILogger<GetCurrentUserRequestsHandler> _logger;
        private readonly IHttpContextService _contextService;

        public GetCurrentUserRequestsHandler(IRequestRepository requestRepository, ILogger<GetCurrentUserRequestsHandler> logger,
            IHttpContextService contextService)
        {
            _requestRepository = requestRepository;
            _logger = logger;
            _contextService = contextService;
        }

        public async Task<List<RequestDto>?> Handle(GetCurrentUserRequests query, CancellationToken cancellationToken)
        {
            var userId = _contextService.GetCurrentUserId();
            var roleName = _contextService.GetCurrentUserRoleName();

            var requests = new List<RequestDto>();

            if (roleName == "Athlete")
                requests = await _requestRepository.GetCurrentAthletesRequestsAsync(userId, query.PageNumber, query.PageSize, query.SearchPhrase, query.IsDescending);
            else if (roleName == "Coach")
                requests = await _requestRepository.GetRequestsForCurrentCoachAsync(userId, query.PageNumber, query.PageSize, query.SearchPhrase, query.IsDescending);
            else
                throw new InvalidOperationException("Cannot find requests");

            return requests;
        }
    }
}
