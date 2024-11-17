using Domain.Entities;
using Fitness.Shared.DTOs;
using MediatR;

namespace Fitness.Application.Reports.Commands
{
    public sealed record CreateReport(
        string? Description,
        DateTime? DateTime,
        int? TariffId,
        int AthleteId,
        List<CreateExerciseDto>? Exercises
        ) : IRequest;
}