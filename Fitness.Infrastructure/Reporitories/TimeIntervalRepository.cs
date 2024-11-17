using Domain.Entities;
using Fitness.Domain.Repositories;
using Fitness.Shared.DTOs;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data.SqlClient;

public class TimeIntervalRepository : ITimeIntervalRepository
{
    private readonly string _connectionString;

    public TimeIntervalRepository(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionString:DeffaultConntection"];
    }

    public async Task<List<TimeIntervalDto>?> GetAsync(int currentUserId, int pageNumber, int pageSize, bool isDescending)
    {
        var timeIntervals = new List<TimeIntervalDto>();

        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var orderBy = isDescending ? "DESC" : "ASC";
                var offset = (pageNumber - 1) * pageSize;

                var query = $@"
                    SELECT ""Id"", ""StartTime"", ""EndTime""
                    FROM ""Fitness"".""TimeIntervals""
                    WHERE ""UserId"" = @UserId
                    ORDER BY ""StartTime"" {orderBy}
                    OFFSET @Offset ROWS
                    LIMIT @PageSize;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", currentUserId);
                    command.Parameters.AddWithValue("@Offset", offset);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            timeIntervals.Add(new TimeIntervalDto(
                                Id: reader.IsDBNull(0) ? null : reader.GetInt32(0),
                                Start: reader.GetDateTime(1),
                                End: reader.GetDateTime(2)
                            ));
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving time intervals.", ex);
        }

        return timeIntervals.Count > 0 ? timeIntervals : null;
    }

    public async Task UpdateAsync(TimeInterval timeInterval)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new NpgsqlCommand(@"
                    UPDATE ""Fitness"".""TimeIntervals"" 
                    SET ""StartTime"" = @StartTime, ""EndTime"" = @EndTime 
                    WHERE ""Id"" = @Id", connection);

                command.Parameters.AddWithValue("@StartTime", timeInterval.StartTime);
                command.Parameters.AddWithValue("@EndTime", timeInterval.EndTime);
                command.Parameters.AddWithValue("@Id", timeInterval.Id);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException("No time interval found with the specified ID.");
                }
            }
        }
        catch (SqlException ex)
        {
            throw new InvalidOperationException("An error occurred while updating the time interval.", ex);
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new NpgsqlCommand(@"DELETE FROM ""Fitness"".""TimeIntervals"" WHERE ""Id"" = @Id", connection);

                command.Parameters.AddWithValue("@Id", id);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException("No time interval found with the specified ID.");
                }
            }
        }
        catch (SqlException ex)
        {
            throw new InvalidOperationException("An error occurred while deleting the time interval.", ex);
        }
    }

    public async Task AddAsync(TimeInterval timeInterval)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new NpgsqlCommand(@"
                    INSERT INTO ""Fitness"".""TimeIntervals"" (""StartTime"", ""EndTime"", ""UserId"") 
                    VALUES (@StartTime, @EndTime, @UserId); 
                    SELECT LASTVAL();", connection);

                command.Parameters.AddWithValue("@StartTime", timeInterval.StartTime);
                command.Parameters.AddWithValue("@EndTime", timeInterval.EndTime);
                command.Parameters.AddWithValue("@UserId", timeInterval.UserId);

                // Получаем ID нового временного интервала
                var newId = Convert.ToInt32(await command.ExecuteScalarAsync());
                timeInterval.Id = newId; // Устанавливаем ID в объекте временного интервала
            }
        }
        catch (SqlException ex)
        {
            throw new InvalidOperationException("An error occurred while adding the time interval.", ex);
        }
    }
}