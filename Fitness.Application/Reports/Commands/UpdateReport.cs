using Domain.Entities;
using Fitness.Shared.DTOs;
using MediatR;

namespace Fitness.Application.Reports.Commands
{
    public sealed record UpdateReport(
        int Id,
        string? Description,
        DateTime? DateTime,
        int TariffId,
        List<EditExerciseDto>? Exercises
        ) : IRequest;
}
