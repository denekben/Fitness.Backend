using Fitness.Application.ApplicationServices;
using Fitness.Shared.DTOs;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data.SqlClient;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace Fitness.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private const int _keySize = 64;
        private const int _iterations = 350000;
        private readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;
        private readonly string _connection;
        private readonly double _refreshTokenLifeTime;

        public AuthService(IConfiguration configuration)
        {
            _refreshTokenLifeTime = Convert.ToDouble(configuration["JWT:RefreshTokenLifeTime"]);
            _connection = configuration["ConnectionString:DeffaultConntection"];
        }

        public async Task AssignUserToRole(int id, string role)
        {
            using (var connection = new NpgsqlConnection(_connection))
            {
                await connection.OpenAsync();

                // Получаем идентификатор роли по имени
                string getRoleIdQuery = @"
                    SELECT ""Id""
                    FROM ""Fitness"".""Roles""
                    WHERE ""RoleName"" = @RoleName;";

                int roleId;
                using (var command = new NpgsqlCommand(getRoleIdQuery, connection))
                {
                    command.Parameters.AddWithValue("@RoleName", role);

                    var result = await command.ExecuteScalarAsync();
                    if (result == null)
                    {
                        throw new KeyNotFoundException("Роль не найдена.");
                    }

                    roleId = Convert.ToInt32(result);
                }

                // Обновляем запись в PersonalInformation с новым RoleId
                string updateUserRoleQuery = @"
                    UPDATE ""Fitness"".""Users""
                    SET ""RoleId"" = @RoleId
                    WHERE ""Id"" = @UserId;";

                using (var command = new NpgsqlCommand(updateUserRoleQuery, connection))
                {
                    command.Parameters.AddWithValue("@RoleId", roleId);
                    command.Parameters.AddWithValue("@UserId", id);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        throw new InvalidOperationException("Не удалось изменить роль");
                    }
                }
            }
        }

        public async Task<int> CreateUserAsync(string firstName, string lastName, string? phone, string password, string email, DateTime? dateOfBirth, string? sex, string rolename)
        {
            if (await UserExistsByEmail(email))
                throw new AuthenticationException("Пользователь с таким email уже существует.");

            using (var connection = new NpgsqlConnection(_connection))
            {
                await connection.OpenAsync();

                // Хэшируем пароль
                string hashedPassword = HashPassword(password);

                // Создаем запись в таблице PersonalInformation
                string insertPersonalInfoQuery = @"
                    INSERT INTO ""Fitness"".""Users"" (""FirstName"", ""LastName"", ""Phone"", ""Email"", ""BirthDate"", ""Sex"", ""PasswordHash"")
                    VALUES (@FirstName, @LastName, @Phone, @Email, @BirthDate, @Sex, @PasswordHash)
                    RETURNING ""Id"";";

                using (var command = new NpgsqlCommand(insertPersonalInfoQuery, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Phone", (object)phone ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@BirthDate", (object)dateOfBirth ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Sex", (object)sex ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                    // Получаем идентификатор созданной записи
                    var userId = (int?)await command.ExecuteScalarAsync()
                        ?? throw new AuthenticationException("Нельзя создать пользователя");

                    await AssignUserToRole(userId, rolename);

                    return userId; // Возвращаем идентификатор пользователя
                }
            }
        }

        // Функция для хеширования пароля
        public string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(_keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                _iterations,
                _hashAlgorithm,
                _keySize);

            // Объединяем хеш и соль в одну строку, разделяя их двоеточием
            return $"{Convert.ToHexString(hash)}:{Convert.ToHexString(salt)}";
        }

        // Функция для верификации пароля
        public bool VerifyPassword(string password, string storedHashWithSalt)
        {
            // Разделяем хеш и соль
            var parts = storedHashWithSalt.Split(':');
            if (parts.Length != 2)
                throw new FormatException("Неверный формат хеша пароля.");

            var storedHash = parts[0];
            var storedSalt = parts[1];

            var saltBytes = Convert.FromHexString(storedSalt);
            var hashToCheck = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                saltBytes,
                _iterations,
                _hashAlgorithm,
                _keySize);

            return storedHash.Equals(Convert.ToHexString(hashToCheck), StringComparison.OrdinalIgnoreCase);
        }

        public async Task<bool> UserExistsByEmail(string email)
        {
            using(var connection = new NpgsqlConnection(_connection)) { 
                await connection.OpenAsync();

                var query = @"
                    SELECT COUNT(1) FROM ""Fitness"".""Users"" WHERE ""Email"" = @Email;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    var result = await command.ExecuteScalarAsync();

                    return Convert.ToInt32(result) > 0;
                }
            }
        }

        public async Task<IdentityDto> GetUserDetailsByEmailAsync(string email)
        {
            using (var connection = new NpgsqlConnection(_connection))
            {
                await connection.OpenAsync();

                string query = @"
                    SELECT u.""Id"", u.""FirstName"", u.""LastName"", u.""Phone"", u.""BirthDate"", u.""Sex"", r.""RoleName"" AS ""Role"", u.""RefreshToken"", u.""RefreshTokenExpires""
                    FROM ""Fitness"".""Users"" u
                    JOIN ""Fitness"".""Roles"" r ON u.""RoleId"" = r.""Id""
                    WHERE u.""Email"" = @Email;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new(
                                reader.GetInt32(0), // Id
                                reader.GetString(1), // FirstName
                                reader.GetString(2), // LastName
                                reader.IsDBNull(3) ? null : reader.GetString(3), // Phone
                                reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4), // DateOfBirth
                                reader.IsDBNull(5) ? null : reader.GetString(5), // Sex
                                reader.GetString(6), //RoleName
                                reader.IsDBNull(7) ? null : reader.GetString(7), //RefreshToken
                                reader.IsDBNull(8) ? (DateTime?)null : reader.GetDateTime(8) // RefreshTokenExpires
                            );
                        }
                        else
                        {
                            throw new KeyNotFoundException("Пользователь не найден.");
                        }
                    }
                }
            }
        }

        public async Task SigninUserAsync(string email, string password)
        {
            using (var connection = new NpgsqlConnection(_connection))
            {
                await connection.OpenAsync();

                // Получаем хэш пароля и соль по email
                string query = @"
                    SELECT ""PasswordHash""
                    FROM ""Fitness"".""Users""
                    WHERE ""Email"" = @Email;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    var storedPasswordHash = await command.ExecuteScalarAsync()
                        ?? throw new AuthenticationException("Неверный email или password");

                    if(!VerifyPassword(password, (string)storedPasswordHash))
                        throw new AuthenticationException("Неверный email или password");
                }
            }
        }

        public async Task UpdateRefreshToken(string email, string refreshToken)
        {
            var user = await GetUserDetailsByEmailAsync(email);

            var refreshTokenExpires = DateTime.UtcNow.AddDays(_refreshTokenLifeTime);

            using (var connection = new NpgsqlConnection(_connection))
            {
                await connection.OpenAsync();

                string query = @"
                    UPDATE ""Fitness"".""Users""
                    SET ""RefreshToken"" = @RefreshToken,
                        ""RefreshTokenExpires"" = @RefreshTokenExpires
                    WHERE ""Id"" = @Id;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RefreshToken", refreshToken);
                    command.Parameters.AddWithValue("@RefreshTokenExpires", refreshTokenExpires);
                    command.Parameters.AddWithValue("@Id", user.Id);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        throw new InvalidOperationException("Не удалось обновить refresh token.");
                    }
                }
            }
        }

        public async Task<bool> IsRefreshTokenValid(string email, string refreshToken)
        {
            var user = await GetUserDetailsByEmailAsync(email);
            return (user.RefreshToken == refreshToken && user.RefreshTokenExpires > DateTime.UtcNow);
        }
    }
}
