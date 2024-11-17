using Domain.Entities;
using Fitness.Domain.Repositories;
using Fitness.Shared.DTOs;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

public class RequestRepository : IRequestRepository
{
    private readonly string _connectionString;

    public RequestRepository(IConfiguration configuratgion)
    {
        _connectionString = configuratgion["ConnectionString:DeffaultConntection"];
    }

    public async Task<List<RequestDto>?> GetCurrentAthletesRequestsAsync(int athleteId, int pageNumber, int pageSize, string searchPhrase, bool isDescending)
    {
        var requests = new List<RequestDto>();

        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var orderBy = isDescending ? "DESC" : "ASC";
                var offset = (pageNumber - 1) * pageSize;

                var query = $@"
                    SELECT r.""Id"", r.""DateTime"", r.""Subject"",
                           a.""Id"" AS ""AthleteId"", a.""FirstName"" AS ""AthleteFirstName"", a.""LastName"" AS ""AthleteLastName"",
                           a.""Phone"" AS ""AthletePhone"", a.""BirthDate"" AS ""AthleteBirthDate"", a.""Sex"" AS ""AthleteSex"",
                           t.""Id"" AS ""TariffId"", t.""Price"", t.""Description"" AS ""TariffDescription"", r.""IsAccepted""
                    FROM ""Fitness"".""Requests"" r
                    JOIN ""Fitness"".""Users"" a ON r.""CoachId"" = a.""Id""
                    LEFT JOIN ""Fitness"".""Tariffs"" t ON r.""TariffId"" = t.""Id""
                    WHERE r.""AthleteId"" = @AthleteId
                      AND (@SearchPhrase IS NULL OR r.""Subject"" ILIKE '%' || @SearchPhrase || '%')
                    ORDER BY 
                        CASE 
                            WHEN t.""Price"" IS NULL OR t.""Price"" = 0 THEN 1 
                            ELSE 0 
                        END, 
                        t.""Price"" {orderBy}
                    OFFSET @Offset 
                    LIMIT @PageSize;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AthleteId", athleteId);
                    command.Parameters.AddWithValue("@SearchPhrase", string.IsNullOrWhiteSpace(searchPhrase) ? string.Empty : searchPhrase);
                    command.Parameters.AddWithValue("@Offset", offset);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            requests.Add(new RequestDto(
                                Id: reader.GetInt32(0),
                                DateTime: reader.IsDBNull(1) ? (DateTime?)null : reader.GetDateTime(1),
                                Subject: reader.IsDBNull(2) ? null : reader.GetString(2),
                                Profile: new ProfileDto(
                                    Id: reader.GetInt32(3),
                                    FirstName: reader.GetString(4),
                                    LastName: reader.GetString(5),
                                    Phone: reader.IsDBNull(6) ? null : reader.GetString(6),
                                    BirthDate: reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                                    Sex: reader.IsDBNull(8) ? null : reader.GetString(8)
                                ),
                                Tariff: new TariffDto(
                                    Id: reader.IsDBNull(9) ? 0 : reader.GetInt32(9), // Tariff Id
                                    Price: reader.GetFloat(10), // Tariff Price
                                    Description: reader.IsDBNull(11) ? null : reader.GetString(11) // Tariff Description
                                ),
                                IsAccepted: reader.GetBoolean(12)
                            ));
                        }
                    }
                }
            }
        }
        catch (NpgsqlException ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving athletes' requests.", ex);
        }

        return requests.Count > 0 ? requests : null;
    }

    public async Task<List<RequestDto>?> GetRequestsForCurrentCoachAsync(int coachId, int pageNumber, int pageSize, string searchPhrase, bool isDescending)
    {
        var requests = new List<RequestDto>();

        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var orderBy = isDescending ? "DESC" : "ASC";
                var offset = (pageNumber - 1) * pageSize;

                var query = $@"
                    SELECT r.""Id"", r.""DateTime"", r.""Subject"",
                           c.""Id"" AS ""CoachId"", c.""FirstName"" AS ""AthleteFirstName"", c.""LastName"" AS ""AthleteLastName"",
                           c.""Phone"" AS ""AthletePhone"", c.""BirthDate"" AS ""AthleteBirthDate"", c.""Sex"" AS ""AthleteSex"",
                           t.""Id"" AS ""TariffId"", t.""Price"", t.""Description"" AS ""TariffDescription"", r.""IsAccepted""
                    FROM ""Fitness"".""Requests"" r
                    JOIN ""Fitness"".""Users"" c ON r.""AthleteId"" = c.""Id""
                    LEFT JOIN ""Fitness"".""Tariffs"" t ON r.""TariffId"" = t.""Id""
                    WHERE r.""CoachId"" = @CoachId
                      AND (@SearchPhrase IS NULL OR r.""Subject"" ILIKE '%' || @SearchPhrase || '%')
                    ORDER BY 
                        CASE 
                            WHEN t.""Price"" IS NULL OR t.""Price"" = 0 THEN 1 
                            ELSE 0 
                        END, 
                        t.""Price"" {orderBy}
                    OFFSET @Offset 
                    LIMIT @PageSize;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CoachId", coachId);
                    command.Parameters.AddWithValue("@SearchPhrase", string.IsNullOrWhiteSpace(searchPhrase) ? string.Empty : searchPhrase);
                    command.Parameters.AddWithValue("@Offset", offset);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            requests.Add(new RequestDto(
                                Id: reader.GetInt32(0),
                                DateTime: reader.IsDBNull(1) ? (DateTime?)null : reader.GetDateTime(1),
                                Subject: reader.IsDBNull(2) ? null : reader.GetString(2),
                                Profile: new ProfileDto(
                                    Id: reader.GetInt32(3),
                                    FirstName: reader.GetString(4),
                                    LastName: reader.GetString(5),
                                    Phone: reader.IsDBNull(6) ? null : reader.GetString(6),
                                    BirthDate: reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                                    Sex: reader.IsDBNull(8) ? null : reader.GetString(8)
                                ),
                                Tariff: reader.IsDBNull(9) ? null : new TariffDto(
                                    Id: reader.GetInt32(9), // Tariff Id
                                    Price: reader.IsDBNull(10) ? 0.0f : reader.GetFloat(10), // Tariff Price
                                    Description: reader.IsDBNull(11) ? null : reader.GetString(11) // Tariff Description
                                ),
                                IsAccepted: reader.GetBoolean(12)
                            ));
                        }
                    }
                }
            }
        }
        catch (NpgsqlException ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving coach's requests.", ex);
        }

        return requests.Count > 0 ? requests : null;
    }

    public async Task UpdateAsync(Request request)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new NpgsqlCommand(@"
                    UPDATE ""Fitness"".""Requests"" 
                    SET ""Subject"" = @Subject,
                        ""DateTime"" = @DateTime,
                        ""TariffId"" = @TariffId
                    WHERE ""Id"" = @Id;", connection);

                command.Parameters.AddWithValue("@Subject", request.Subject);
                command.Parameters.AddWithValue("@DateTime", request.DateTime);
                command.Parameters.AddWithValue("@Id", request.Id);
                command.Parameters.AddWithValue("@TariffId", (request.TariffId==null) ? (object)DBNull.Value : request.TariffId);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException("No request found with the specified ID.");
                }
            }
        }
        catch (SqlException ex)
        {
            throw new InvalidOperationException("An error occurred while updating the request.", ex);
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new NpgsqlCommand(@"DELETE FROM ""Fitness"".""Requests"" WHERE ""Id"" = @Id", connection);

                command.Parameters.AddWithValue("@Id", id);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException("No request found with the specified ID.");
                }
            }
        }
        catch (SqlException ex)
        {
            throw new InvalidOperationException("An error occurred while deleting the request.", ex);
        }
    }

    public async Task AddAsync(Request request)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new NpgsqlCommand(@"
                    INSERT INTO ""Fitness"".""Requests"" (""DateTime"", ""Subject"", ""CoachId"", ""AthleteId"", ""TariffId"", ""IsAccepted"") 
                    VALUES (@DateTime, @Subject, @CoachId, @AthleteId, @TariffId, @IsAccepted) 
                    RETURNING ""Id"";", connection);

                command.Parameters.AddWithValue("@DateTime", request.DateTime);
                command.Parameters.AddWithValue("@Subject", request.Subject);
                command.Parameters.AddWithValue("@CoachId", request.CoachId);
                command.Parameters.AddWithValue("@AthleteId", request.AthleteId);
                command.Parameters.AddWithValue("@TariffId", request.TariffId);
                command.Parameters.AddWithValue("@IsAccepted", false);

                // Получаем ID нового запроса
                var newId = Convert.ToInt32(await command.ExecuteScalarAsync());
                request.Id = newId; // Устанавливаем ID в объекте запроса
            }
        }
        catch (SqlException ex)
        {
            throw new InvalidOperationException("An error occurred while adding the request.", ex);
        }
    }

    public async Task AcceptRequest(int requestId)
    {
        try
        {
            using(var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new NpgsqlCommand(@"
                    UPDATE ""Fitness"".""Requests""
                    SET ""IsAccepted"" = @IsAccepted
                    WHERE ""Id"" = @Id;" , connection);

                command.Parameters.AddWithValue("@Id", requestId);
                command.Parameters.AddWithValue("@IsAccepted", true);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException("No request found with the specified ID.");
                }
            }
        }
        catch (SqlException ex)
        {
            throw new InvalidOperationException("An error occurred while accepting the request.", ex);
        }
    }
}