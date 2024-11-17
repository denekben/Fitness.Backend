using Domain.Entities;
using Fitness.Domain.Repositories;
using Fitness.Shared.DTOs;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Diagnostics;

public class TariffRepository : ITariffRepository
{
    private readonly string _connectionString;

    public TariffRepository(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionString:DeffaultConntection"];
    }

    public async Task<List<TariffDto>?> GetAsync(int pageNumber, int pageSize, string searchPhrase, bool isDescending, decimal minPrice, decimal maxPrice)
    {
        var tariffs = new List<TariffDto>();
        string orderBy = isDescending ? "ORDER BY \"Price\" DESC" : "ORDER BY \"Price\" ASC";

        string sql = $@"
            SELECT ""Id"", ""Price"", ""Description"" 
            FROM ""Fitness"".""Tariffs"" 
            WHERE (@SearchPhrase IS NULL OR ""Description"" ILIKE '%' || @SearchPhrase || '%') 
              AND ""Price"" >= @MinPrice 
              AND ""Price"" <= @MaxPrice 
            {orderBy}
            OFFSET @Offset ROWS 
            LIMIT @PageSize;";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@SearchPhrase", string.IsNullOrWhiteSpace(searchPhrase) ? string.Empty : searchPhrase);
                command.Parameters.AddWithValue("@MinPrice", minPrice);
                command.Parameters.AddWithValue("@MaxPrice", maxPrice);
                command.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var tariffDto = new TariffDto(
                            reader.GetInt32(0), // Id
                            reader.GetFloat(1), // Price
                            reader.IsDBNull(2) ? null : reader.GetString(2) // Description
                        );
                        tariffs.Add(tariffDto);
                    }
                }
            }
        }

        return tariffs.Count > 0 ? tariffs : null;
    }

    public async Task UpdateAsync(Tariff tariff)
    {
        string sql = @"
            UPDATE ""Fitness"".""Tariffs"" 
            SET ""Price"" = @Price, 
                ""Description"" = @Description 
            WHERE ""Id"" = @Id;";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", tariff.Id ?? throw new ArgumentNullException(nameof(tariff.Id)));
                command.Parameters.AddWithValue("@Price", (object)tariff.Price);
                command.Parameters.AddWithValue("@Description", tariff.Description ?? (object)DBNull.Value);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task DeleteAsync(int id)
    {
        string sql = @"DELETE FROM ""Fitness"".""Tariffs"" WHERE ""Id"" = @Id;";

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

    public async Task AddAsync(Tariff tariff)
    {
        string sql = @"
            INSERT INTO ""Fitness"".""Tariffs"" (""Price"", ""Description"") 
            VALUES(@Price, @Description); ";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Price", (object)tariff.Price);
                command.Parameters.AddWithValue("@Description", tariff.Description ?? (object)DBNull.Value);

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}