namespace Fitness.Shared.DTOs
{
    public sealed record IdentityDto
    (
        int Id,
        string FirstName,
        string LastName, 
        string? Phone,
        DateTime? BirthDate,
        string? Sex,
        string RoleName,
        string? RefreshToken,
        DateTime? RefreshTokenExpires
    );
}
