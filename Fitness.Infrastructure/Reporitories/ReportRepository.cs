using Domain.Entities;
using Fitness.Domain.Repositories;
using Fitness.Shared.DTOs;
using Microsoft.Extensions.Configuration;
using Npgsql;

public class ReportRepository : IReportRepository
{
    private readonly string _connectionString;
    private readonly IExerciseRepository _exerciseRepository;

    public ReportRepository(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionString:DeffaultConntection"];
    }

    public async Task<List<ReportDto>?> GetReportsAboutCurrentAthleteAsync(int athleteId, int pageNumber, int pageSize, string searchPhrase)
    {
        var reports = new List<ReportDto>();
        string sql = @"
            SELECT r.""Id"", r.""Description"", r.""DateTime"", 
                    t.""Id"" AS ""TariffId"", t.""Price"", t.""Description"" AS ""TariffDescription"",
                    u.""Id"" AS ""CoachId"", u.""FirstName"" AS ""CoachFirstName"", u.""LastName"" AS ""CoachLastName"",
                    u.""Phone"" AS ""CoachPhone"", u.""BirthDate"" AS ""CoachBirthDate"", u.""Sex"" AS ""CoachSex"",
                    u.""RoleId"" AS ""CoachRoleId""
            FROM ""Fitness"".""Reports"" r
            JOIN ""Fitness"".""Tariffs"" t ON r.""TariffId"" = t.""Id""
            JOIN ""Fitness"".""Users"" u ON r.""CoachId"" = u.""Id""
            WHERE r.""AthleteId"" = @AthleteId 
            AND (@SearchPhrase IS NULL OR r.""Description"" ILIKE '%' || @SearchPhrase || '%')
            ORDER BY r.""DateTime"" DESC
            OFFSET @Offset 
            LIMIT @PageSize;";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@AthleteId", athleteId);
                command.Parameters.AddWithValue("@SearchPhrase", string.IsNullOrWhiteSpace(searchPhrase) ? string.Empty : searchPhrase);
                command.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var reportId = reader.GetInt32(0);
                        var reportDto = new ReportDto(
                            reportId,
                            reader.IsDBNull(1) ? null : reader.GetString(1), // Description
                            reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2), // DateTime
                            new List<ExerciseDto>(),
                            new TariffDto(
                                reader.GetInt32(3), // Tariff Id
                                reader.GetFloat(4), // Price
                                reader.IsDBNull(5) ? null : reader.GetString(5) // Tariff Description
                            ),
                            new ProfileDto(
                                reader.GetInt32(6), // Coach Id
                                reader.GetString(7), // Coach FirstName
                                reader.GetString(8), // Coach LastName
                                reader.IsDBNull(9) ? null : reader.GetString(9), // Coach Phone
                                reader.IsDBNull(10) ? (DateTime?)null : reader.GetDateTime(10), // Coach BirthDate
                                reader.IsDBNull(11) ? null : reader.GetString(11) // Coach Sex
                            )
                        );

                        // Получение списка занятий для текущего отчета
                        reportDto.Exercises.AddRange(await GetExercisesForReportAsync(reportId));

                        reports.Add(reportDto);
                    }
                }
            }
        }

        return reports.Count > 0 ? reports : null;
    }

    public async Task<List<ReportDto>?> GetReportsCreatedByCurrentCoachAsync(int coachId, int pageNumber, int pageSize, string searchPhrase)
    {
        var reports = new List<ReportDto>();
        string sql = @"
            SELECT r.""Id"", r.""Description"", r.""DateTime"",
                   t.""Id"" AS ""TariffId"", t.""Price"", t.""Description"" AS ""TariffDescription"",
                   u.""Id"" AS ""AthleteId"", u.""FirstName"" AS ""AthleteFirstName"", u.""LastName"" AS ""AthleteLastName"",
                   u.""Phone"" AS ""AthletePhone"", u.""BirthDate"" AS ""AthleteBirthDate"", u.""Sex"" AS ""AthleteSex"",
                   u.""RoleId"" AS ""AthleteRoleId""
            FROM ""Fitness"".""Reports"" r
            JOIN ""Fitness"".""Tariffs"" t ON r.""TariffId"" = t.""Id""
            JOIN ""Fitness"".""Users"" u ON r.""AthleteId"" = u.""Id""
            WHERE (r.""CoachId"" = @CoachId) 
            AND (@SearchPhrase IS NULL OR r.""Description"" ILIKE '%' || @SearchPhrase || '%')
            ORDER BY r.""DateTime"" DESC
            OFFSET @Offset 
            LIMIT @PageSize;";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@CoachId", coachId);
                command.Parameters.AddWithValue("@SearchPhrase", string.IsNullOrWhiteSpace(searchPhrase) ? string.Empty : searchPhrase);
                command.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var reportId = reader.GetInt32(0);
                        var reportDto = new ReportDto(
                            reportId,
                            reader.IsDBNull(1) ? null : reader.GetString(1), // Description
                            reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2), // DateTime
                            new List<ExerciseDto>(), 
                            new TariffDto(
                                reader.GetInt32(3), // Tariff Id
                                reader.GetFloat(4), // Price
                                reader.IsDBNull(5) ? null : reader.GetString(5) // Tariff Description
                            ),
                            new ProfileDto(
                                reader.GetInt32(6), // Athlete Id
                                reader.GetString(7), // Athlete FirstName
                                reader.GetString(8), // Athlete LastName
                                reader.IsDBNull(9) ? null : reader.GetString(9), // Athlete Phone
                                reader.IsDBNull(10) ? (DateTime?)null : reader.GetDateTime(10), // Athlete BirthDate 
                                reader.IsDBNull(11) ? null : reader.GetString(11) // Athlete Sex 
                            )
                        );

                        // Получение списка занятий для текущего отчета
                        reportDto.Exercises.AddRange(await GetExercisesForReportAsync(reportId));

                        reports.Add(reportDto);
                    }
                }
            }
        }

        return reports.Count > 0 ? reports : null;
    }

    private async Task<List<ExerciseDto>> GetExercisesForReportAsync(int reportId)
    {
        var exercises = new List<ExerciseDto>();
        string sql = @"
            SELECT e.""Id"", e.""RepeatQuantity"", e.""SetQuantity"", e.""Name"" 
            FROM ""Fitness"".""Exercises"" e
            WHERE e.""ReportId"" = @ReportId;";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ReportId", reportId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var exerciseDto = new ExerciseDto(
                            reader.GetInt32(0),
                            reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1), // RepeatQuantity
                            reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2), // SetQuantity
                            reader.IsDBNull(3) ? null : reader.GetString(3) // Name
                        );

                        exercises.Add(exerciseDto);
                    }
                }
            }
        }

        return exercises;
    }

    public async Task UpdateAsync(int id, string? description, DateTime? dateTime, int tariffId, List<EditExerciseDto>? exercises)
    {
        // Шаг 1: Обновление отчета
        string updateSql = @"
    UPDATE ""Fitness"".""Reports""
    SET ""Description"" = @Description,
        ""DateTime"" = @DateTime,
        ""TariffId"" = @TariffId
    WHERE ""Id"" = @Id;";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Обновляем отчет
            using (var command = new NpgsqlCommand(updateSql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Description", description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@DateTime", dateTime ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@TariffId", tariffId);

                await command.ExecuteNonQueryAsync();
            }

            // Шаг 2: Удаление старых упражнений
            string deleteSql = @"
        DELETE FROM ""Fitness"".""Exercises""
        WHERE ""ReportId"" = @ReportId;";

            using (var deleteCommand = new NpgsqlCommand(deleteSql, connection))
            {
                deleteCommand.Parameters.AddWithValue("@ReportId", id);
                await deleteCommand.ExecuteNonQueryAsync();
            }

            // Шаг 3: Добавление новых упражнений
            if (exercises != null && exercises.Count > 0)
            {
                string insertSql = @"
            INSERT INTO ""Fitness"".""Exercises"" (""RepeatQuantity"", ""SetQuantity"", ""Name"", ""ReportId"")
            VALUES (@RepeatQuantity, @SetQuantity, @Name, @ReportId);";

                foreach (var exercise in exercises)
                {
                    using (var insertCommand = new NpgsqlCommand(insertSql, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@RepeatQuantity", exercise.RepeatQuantity ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@SetQuantity", exercise.SetQuantity ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@Name", exercise.Name ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@ReportId", id);

                        await insertCommand.ExecuteNonQueryAsync();
                    }
                }
            }
        }
    }

    public async Task DeleteAsync(int id)
    {
        string sql = @"DELETE FROM ""Fitness"".""Reports"" WHERE ""Id"" = @Id;";

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

    public async Task AddAsync(Report report, List<CreateExerciseDto> exercises)
    {
        string insertReportSql = @"
        INSERT INTO ""Fitness"".""Reports"" (""Description"", ""DateTime"", ""TariffId"", ""CoachId"", ""AthleteId"")
        VALUES (@Description, @DateTime, @TariffId, @CoachId, @AthleteId) 
        RETURNING ""Id"";"; // Возвращаем Id нового отчета

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Шаг 1: Вставка нового отчета
            int newReportId;
            using (var command = new NpgsqlCommand(insertReportSql, connection))
            {
                command.Parameters.AddWithValue("@Description", report.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@DateTime", report.DateTime ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@TariffId", report.TariffId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CoachId", report.CoachId);
                command.Parameters.AddWithValue("@AthleteId", report.AthleteId);

                // Выполняем команду и получаем Id нового отчета
                newReportId = (int)await command.ExecuteScalarAsync();
            }

            // Добавление новых упражнений
            if (exercises != null && exercises.Count > 0)
            {
                string insertSql = @"
            INSERT INTO ""Fitness"".""Exercises"" (""RepeatQuantity"", ""SetQuantity"", ""Name"", ""ReportId"")
            VALUES (@RepeatQuantity, @SetQuantity, @Name, @ReportId);";

                foreach (var exercise in exercises)
                {
                    using (var insertCommand = new NpgsqlCommand(insertSql, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@RepeatQuantity", exercise.RepeatQuantity ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@SetQuantity", exercise.SetQuantity ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@Name", exercise.Name ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@ReportId", newReportId);

                        await insertCommand.ExecuteNonQueryAsync();
                    }
                }
            }
        }
    }
}