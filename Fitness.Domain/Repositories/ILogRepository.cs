using Fitness.Shared.DTOs.Logs;

namespace Fitness.Infrastructure.Repositories
{
    public interface ILogRepository
    {
        Task<List<UserLog>> GetUserLogsAsync();
        Task<List<RequestLog>> GetRequestLogsAsync();
        Task<List<ReportLog>> GetReportLogsAsync();
    }
}
