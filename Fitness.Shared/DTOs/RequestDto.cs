namespace Fitness.Shared.DTOs
{
    public sealed record RequestDto
    (
        int Id,
        DateTime? DateTime,
        string? Subject,
        ProfileDto Profile,
        TariffDto? Tariff,
        bool IsAccepted
    );
}
