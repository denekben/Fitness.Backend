using Fitness.Shared.DTOs;

namespace Fitness.Application.ApplicationServices
{
    public interface IAuthService
    {
        Task AssignUserToRole(int id, string role);
        Task<int> CreateUserAsync(string firstName, string lastName, string? phone, string password, string email, DateTime? dateOfBirth, string? sex, string roleName);
        string HashPassword(string password);
        Task<bool> UserExistsByEmail(string email);
        Task<IdentityDto> GetUserDetailsByEmailAsync(string email);
        Task SigninUserAsync(string email, string password);

        Task UpdateRefreshToken(string email, string refreshToken);
        Task<bool> IsRefreshTokenValid(string email, string refreshToken);
    }
}
