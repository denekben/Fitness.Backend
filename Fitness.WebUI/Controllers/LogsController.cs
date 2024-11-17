using Fitness.Infrastructure.Repositories;
using Fitness.Shared.DTOs.Logs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.WebUI.Controllers
{
    [ApiController]
    [Authorize(Roles = "Coach")]
    [Route("api/logs")]
    public class LogsController : ControllerBase
    {
        private readonly ILogRepository _logRepository;

        public LogsController(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        [HttpGet]
        [Route("users")]
        public async Task<ActionResult<List<UserLog>?>> GetUserLogs()
        {
            return await _logRepository.GetUserLogsAsync();
        }

        [HttpGet]
        [Route("reports")]
        public async Task<ActionResult<List<ReportLog>?>> GetReportLogs()
        {
            return await _logRepository.GetReportLogsAsync();
        }

        [HttpGet]
        [Route("requests")]
        public async Task<ActionResult<List<RequestLog>?>> GetRequestLogs()
        {
            return await _logRepository.GetRequestLogsAsync();
        }
    }
}
