using Fitness.Shared.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Fitness.Application.Exercises.Queries
{
    public sealed record GetExercises(
        int ReportId,
        bool IdDescending,
        string SearchPhrase = "",
        int PageNumber = 1, 
        int PageSize = 10,
        int MinRepeatQuantity = 0, 
        int MaxRepeatQantity = int.MaxValue, 
        int MinSetQuantity = 0, 
        int MaxSetQuantity = int.MaxValue
        ) : IRequest<List<ExerciseDto>?>;
}
