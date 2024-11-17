using Domain.Entities;
using Fitness.Shared.DTOs;

namespace Fitness.Domain.Repositories
{
    public interface IReportRepository
    {
        Task<List<ReportDto>?> GetReportsAboutCurrentAthleteAsync(int athleteId, int pageNumber, int pageSize, string searchPhrase);
        Task<List<ReportDto>?> GetReportsCreatedByCurrentCoachAsync(int coachId, int pageNumber, int pageSize, string searchPhrase);
        Task UpdateAsync(int id, string? description, DateTime? dateTime, int tariffId, List<EditExerciseDto>? exercises);
        Task DeleteAsync(int id);
        Task AddAsync(Report report, List<CreateExerciseDto> exercises);
    }
}