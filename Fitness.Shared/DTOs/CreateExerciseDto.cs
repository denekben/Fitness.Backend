namespace Fitness.Shared.DTOs
{
    public sealed record CreateExerciseDto(
        int? RepeatQuantity,
        int? SetQuantity,
        string? Name
    );
}
