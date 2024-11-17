using MediatR;

namespace Fitness.Application.Exercises.Commands
{
    public sealed record DeleteExercise(int Id) : IRequest;
}
