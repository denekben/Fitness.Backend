using Domain.Entities;
using Fitness.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.Exercises.Commands.Handlers
{
    internal sealed class UpdateExerciseHandler : IRequestHandler<UpdateExercise>
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly ILogger<UpdateExerciseHandler> _logger;

        public UpdateExerciseHandler(IExerciseRepository exerciseRepository, ILogger<UpdateExerciseHandler> logger)
        {
            _exerciseRepository = exerciseRepository;
            _logger = logger;
        }

        public async Task Handle(UpdateExercise command, CancellationToken cancellationToken)
        {
            await _exerciseRepository.UpdateAsync(new Exercise(command.Id, command.RepeatQuantity, command.SetQuantity, command.Name, command.ReportId));
            _logger.LogInformation("Exercise created");
        }
    }
}
