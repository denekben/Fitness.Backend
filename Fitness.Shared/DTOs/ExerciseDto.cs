namespace Fitness.Shared.DTOs
{
    public sealed record ExerciseDto
    (
        int Id,
        int? RepeatQuantity,
        int? SetQuantity,
        string? Name
    );
}
