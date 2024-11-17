using Domain.Entities;
using Fitness.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.Exercises.Commands.Handlers
{
    internal sealed class DeleteExerciseHandler : IRequestHandler<DeleteExercise>
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly ILogger<DeleteExerciseHandler> _logger;

        public DeleteExerciseHandler(IExerciseRepository exerciseRepository, ILogger<DeleteExerciseHandler> logger)
        {
            _exerciseRepository = exerciseRepository;
            _logger = logger;
        }

        public async Task Handle(DeleteExercise command, CancellationToken cancellationToken)
        {
            await _exerciseRepository.DeleteAsync(command.Id);
            _logger.LogInformation("Exercise deleted");
        }
    }
}
