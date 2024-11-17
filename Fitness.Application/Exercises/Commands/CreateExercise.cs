using MediatR;

namespace Fitness.Application.Exercises.Commands
{
    public sealed record CreateExercise(
        int? RepeatQuantity,
        int? SetQuantity,
        string? Name,
        int ReportId
    ) : IRequest;
}