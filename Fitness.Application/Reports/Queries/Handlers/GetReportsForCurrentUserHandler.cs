using Fitness.Domain.Repositories;
using Fitness.Shared.DTOs;
using Fitness.Shared.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.Reports.Queries.Handlers
{
    internal sealed class GetReportsForCurrentUserHandler : IRequestHandler<GetReportsForCurrentUser, List<ReportDto>?>
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILogger<GetReportsForCurrentUserHandler> _logger;
        private readonly IHttpContextService _contextService;

        public GetReportsForCurrentUserHandler(IReportRepository reportRepository, 
            ILogger<GetReportsForCurrentUserHandler> logger, IHttpContextService contextService)
        {
            _reportRepository = reportRepository;
            _logger = logger;
            _contextService = contextService;
        }

        public async Task<List<ReportDto>?> Handle(GetReportsForCurrentUser query, CancellationToken cancellationToken)
        {
            var userId = _contextService.GetCurrentUserId();
            var roleName = _contextService.GetCurrentUserRoleName();

            var reports = new List<ReportDto>();

            if (roleName == "Coach")
                reports = await _reportRepository.GetReportsCreatedByCurrentCoachAsync(userId, query.PageNumber, query.PageSize, query.SearchPhrase);
            else if (roleName == "Athlete")
                reports = await _reportRepository.GetReportsAboutCurrentAthleteAsync(userId, query.PageNumber, query.PageSize, query.SearchPhrase);
            else
                throw new InvalidOperationException("Cannot get report");

            _logger.LogInformation("Reports was found");

            return reports;
        }
    }
}
