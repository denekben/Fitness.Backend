using Npgsql;
using Fitness.Infrastructure.Repositories;
using Fitness.Shared.DTOs.Logs;
using Microsoft.Extensions.Configuration;

namespace Fitness.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly string _connectionString;

        public LogRepository(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionString:DeffaultConntection"];
        }

        public async Task<List<UserLog>> GetUserLogsAsync()
        {
            var logs = new List<UserLog>();
            string query = @"
                SELECT ""Id"", ""UserId"", ""Action"", ""Timestamp"", ""Details""
                FROM ""Fitness"".""Users_Logs"";";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        logs.Add(new UserLog
                        {
                            Id = reader.GetInt32(0),
                            UserId = reader.GetInt32(1),
                            Action = reader.GetString(2),
                            Timestamp = reader.GetDateTime(3),
                            Details = reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
                        });
                    }
                }
            }
            return logs;
        }

        public async Task<List<ReportLog>> GetReportLogsAsync()
        {
            var logs = new List<ReportLog>();
            string query = @"
                SELECT ""Id"", ""ReportId"", ""Action"", ""Timestamp"", ""Details""
                FROM ""Fitness"".""Reports_Logs"";";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        logs.Add(new ReportLog
                        {
                            Id = reader.GetInt32(0),
                            ReportId = reader.GetInt32(1),
                            Action = reader.GetString(2),
                            Timestamp = reader.GetDateTime(3),
                            Details = reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
                        });
                    }
                }
            }
            return logs;
        }

        public async Task<List<RequestLog>> GetRequestLogsAsync()
        {
            var logs = new List<RequestLog>();
            string query = @"
                SELECT ""Id"", ""RequestId"", ""Action"", ""Timestamp"", ""Details""
                FROM ""Fitness"".""Requests_Log"";";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        logs.Add(new RequestLog
                        {
                            Id = reader.GetInt32(0),
                            RequestId = reader.GetInt32(1),
                            Action = reader.GetString(2),
                            Timestamp = reader.GetDateTime(3),
                            Details = reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
                        });
                    }
                }
            }
            return logs;
        }
    }
}