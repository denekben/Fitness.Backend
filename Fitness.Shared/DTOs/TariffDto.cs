namespace Fitness.Shared.DTOs
{
    public sealed record TariffDto
    (
        int Id,
        float Price,
        string? Description
    );
}
