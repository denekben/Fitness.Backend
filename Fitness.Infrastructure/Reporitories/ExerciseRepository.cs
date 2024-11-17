using Domain.Entities;
using Fitness.Domain.Repositories;
using Fitness.Shared.DTOs;
using Microsoft.Extensions.Configuration;
using Npgsql;

public class ExerciseRepository : IExerciseRepository
{
    private readonly string _connectionString;

    public ExerciseRepository(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionString:DeffaultConntection"];
    }

    public async Task<List<ExerciseDto>?> GetAsync(int pageNumber, int pageSize, string searchPhrase, bool idDescending, int minRepeatQuantity, int maxRepeatQuantity, int minSetQuantity, int maxSetQuantity, int reportId)
    {
        try
        {
            var exercises = new List<ExerciseDto>();
            string orderBy = idDescending ? @"ORDER BY ""SetQuantity"" DESC" : @"ORDER BY ""SetQuantity"" ASC";

            string sql = $@"
            SELECT ""Id"", ""RepeatQuantity"", ""SetQuantity"", ""Name"" 
            FROM ""Fitness"".""Exercises"" 
            WHERE (LENGTH(@SearchPhrase) = 0 OR ""Name"" ILIKE '%' || @SearchPhrase || '%') 
              AND ""RepeatQuantity"" >= @MinRepeatQuantity
              AND ""RepeatQuantity"" <= @MaxRepeatQuantity
              AND ""SetQuantity"" >= @MinSetQuantity
              AND ""SetQuantity"" <= @MaxSetQuantity
              AND ""ReportId"" = @ReportId
            {orderBy}
            OFFSET @Offset ROWS 
            LIMIT @PageSize;";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@SearchPhrase", string.IsNullOrWhiteSpace(searchPhrase) ? string.Empty : searchPhrase);
                    command.Parameters.AddWithValue("@MinRepeatQuantity", minRepeatQuantity);
                    command.Parameters.AddWithValue("@MaxRepeatQuantity", maxRepeatQuantity);
                    command.Parameters.AddWithValue("@MinSetQuantity", minSetQuantity);
                    command.Parameters.AddWithValue("@MaxSetQuantity", maxSetQuantity);
                    command.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    command.Parameters.AddWithValue("@ReportId", reportId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var exerciseDto = new ExerciseDto(
                                reader.GetInt32(0), // Id
                                reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1), // RepeatQuantity
                                reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2), // SetQuantity
                                reader.IsDBNull(3) ? null : reader.GetString(3) // Name
                            );
                            exercises.Add(exerciseDto);
                        }
                    }
                }
            }
            return exercises.Count > 0 ? exercises : null;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Cannot get exercise", ex);
        }
    }

    public async Task UpdateAsync(Exercise exercise)
    {
        try
        {
            string sql = @"
                UPDATE ""Fitness"".""Exercises"" 
                SET ""RepeatQuantity"" = @RepeatQuantity, 
                    ""SetQuantity"" = @SetQuantity, 
                    ""Name"" = @Name 
                WHERE ""Id"" = @Id;";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", exercise.Id);
                    command.Parameters.AddWithValue("@RepeatQuantity", exercise.RepeatQuantity.HasValue ? (object)exercise.RepeatQuantity.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@SetQuantity", exercise.SetQuantity.HasValue ? (object)exercise.SetQuantity.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@Name", exercise.Name ?? (object)DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        catch
        {
            throw new InvalidOperationException("Cannot update exercise");
        }
        
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            string sql = @"DELETE FROM ""Fitness"".""Exercises"" WHERE ""Id"" = @Id;";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        catch
        {
            throw new InvalidOperationException("Cannot delete exercise");
        }

    }

    public async Task AddAsync(Exercise exercise)
    {
        try
        {
            string sql = @"
                INSERT INTO ""Fitness"".""Exercises"" (""RepeatQuantity"", ""SetQuantity"", ""Name"", ""ReportId"") 
                VALUES (@RepeatQuantity, @SetQuantity, @Name, @ReportId);";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@RepeatQuantity", exercise.RepeatQuantity.HasValue ? (object)exercise.RepeatQuantity.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@SetQuantity", exercise.SetQuantity.HasValue ? (object)exercise.SetQuantity.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@Name", exercise.Name ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ReportId", exercise.ReportId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        catch
        {
            throw new InvalidOperationException("Cannot add exercise");
        }
    }
}