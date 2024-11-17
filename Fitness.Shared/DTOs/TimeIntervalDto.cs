namespace Fitness.Shared.DTOs
{
    public sealed record TimeIntervalDto(
        int? Id,
        DateTime Start,
        DateTime End
    );
}
