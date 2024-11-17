using Fitness.Domain.Repositories;
using Fitness.Shared.DTOs;
using Microsoft.Extensions.Configuration;
using Npgsql;


namespace Fitness.Infrastructure.Reporitories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionString:DeffaultConntection"];
        }

        public async Task<List<ProfileDto>?> GetCoachesAsync(int pageNumber, int pageSize, string searchPhrase, bool isDescending, string sex)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                    SELECT u.""Id"", u.""FirstName"" AS ""FirstName"", u.""LastName"" AS ""LastName"", 
                           u.""Phone"" AS ""Phone"", u.""BirthDate"" AS ""BirthDate"", u.""Sex"" AS ""Sex""
                    FROM ""Fitness"".""Users"" u
                    JOIN ""Fitness"".""Roles"" r ON u.""RoleId"" = r.""Id""
                    WHERE r.""RoleName"" = 'Coach'
                      AND (u.""FirstName"" ILIKE '%' || @SearchPhrase || '%' OR 
                           u.""LastName"" ILIKE '%' || @SearchPhrase || '%')
                      AND (@Sex IS NULL OR @Sex = '' OR u.""Sex"" = @Sex) -- Фильтрация по полу
                    ORDER BY 
                        CASE WHEN u.""BirthDate"" IS NULL THEN 1 ELSE 0 END, -- Сначала все не null
                        u.""BirthDate"" " + (isDescending ? "DESC" : "ASC") + @"
                    LIMIT @PageSize OFFSET @Offset;";

                var offset = (pageNumber - 1) * pageSize;

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("SearchPhrase", string.IsNullOrWhiteSpace(searchPhrase) ? string.Empty : searchPhrase);
                    command.Parameters.AddWithValue("PageSize", pageSize);
                    command.Parameters.AddWithValue("Offset", offset);
                    command.Parameters.AddWithValue("Sex", string.IsNullOrWhiteSpace(sex) ? string.Empty : sex); // Добавляем параметр для пола

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var coaches = new List<ProfileDto>();
                        while (await reader.ReadAsync())
                        {
                            var coach = new ProfileDto(
                                reader.GetInt32(0), // Id
                                reader.GetString(1), // FirstName
                                reader.GetString(2), // LastName
                                reader.IsDBNull(3) ? null : reader.GetString(3), // Phone
                                reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4), // BirthDate
                                reader.IsDBNull(5) ? null : reader.GetString(5) // Sex
                            );
                            coaches.Add(coach);
                        }
                        return coaches;
                    }
                }
            }
        }

        public async Task<ProfileDto?> GetUserAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                    SELECT u.""Id"", u.""FirstName"" AS ""FirstName"", u.""LastName"" AS ""LastName"", 
                           u.""Phone"" AS ""Phone"", u.""BirthDate"" AS ""BirthDate"", u.""Sex"" AS ""Sex""
                    FROM ""Fitness"".""Users"" u
                    WHERE u.""Id"" = @Id;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new ProfileDto(
                                reader.GetInt32(0), // Id
                                reader.GetString(1), // FirstName
                                reader.GetString(2), // LastName
                                reader.IsDBNull(3) ? null : reader.GetString(3), // Phone
                                reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4), // BirthDate
                                reader.IsDBNull(5) ? null : reader.GetString(5) // Sex
                            );
                        }
                        else
                        {
                            return null; // Возвращаем null, если пользователь не найден
                        }
                    }
                }
            }
        }
    }
}
