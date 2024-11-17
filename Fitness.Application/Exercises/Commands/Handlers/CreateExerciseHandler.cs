using Domain.Entities;
using Fitness.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.Exercises.Commands.Handlers
{
    internal sealed class CreateExerciseHandler : IRequestHandler<CreateExercise>
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly ILogger<CreateExerciseHandler> _logger;

        public CreateExerciseHandler(IExerciseRepository exerciseRepository, ILogger<CreateExerciseHandler> logger)
        {
            _exerciseRepository = exerciseRepository;
            _logger = logger;
        }

        public async Task Handle(CreateExercise command, CancellationToken cancellationToken)
        {
            await _exerciseRepository.AddAsync(new Exercise(command.RepeatQuantity, command.SetQuantity, command.Name, command.ReportId));
            _logger.LogInformation("Exercise created");
        }
    }
}
