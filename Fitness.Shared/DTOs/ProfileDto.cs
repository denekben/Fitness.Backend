namespace Fitness.Shared.DTOs
{
    public sealed record ProfileDto
    (
        int Id,
        string FirstName,
        string LastName,
        string? Phone,
        DateTime? BirthDate,
        string? Sex
    );
}
