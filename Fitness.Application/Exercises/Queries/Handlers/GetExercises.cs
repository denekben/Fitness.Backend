using Fitness.Domain.Repositories;
using Fitness.Shared.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fitness.Application.Exercises.Queries.Handlers
{
    internal sealed class GetExercisesHandler : IRequestHandler<GetExercises, List<ExerciseDto>?>
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly ILogger<GetExercisesHandler> _logger;

        public GetExercisesHandler(IExerciseRepository exerciseRepository, ILogger<GetExercisesHandler> logger)
        {
            _exerciseRepository = exerciseRepository;
            _logger = logger;
        }

        public async Task<List<ExerciseDto>?> Handle(GetExercises query, CancellationToken cancellationToken)
        {
            var exercises =  await _exerciseRepository.GetAsync(
                query.PageNumber, query.PageSize, query.SearchPhrase, query.IdDescending, query.MinRepeatQuantity, query.MaxRepeatQantity, query.MinSetQuantity, query.MaxSetQuantity, query.ReportId);
            _logger.LogInformation("Exercises founded");

            return exercises;
        }
    }
}