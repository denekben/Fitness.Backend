using MediatR;

namespace Fitness.Application.Exercises.Commands
{
    public sealed record UpdateExercise(
        int Id,
        int? RepeatQuantity,
        int? SetQuantity,
        string? Name,
        int ReportId
    ) : IRequest;
}
