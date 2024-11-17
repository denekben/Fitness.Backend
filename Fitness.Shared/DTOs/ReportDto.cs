namespace Fitness.Shared.DTOs
{
    public sealed record ReportDto
    (
        int Id,
        string? Description,
        DateTime? DateTime,
        List<ExerciseDto> Exercises,
        TariffDto Tariff,
        ProfileDto Profile
    );
}
