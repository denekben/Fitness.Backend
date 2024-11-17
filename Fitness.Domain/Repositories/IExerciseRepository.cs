using Domain.Entities;
using Fitness.Shared.DTOs;

namespace Fitness.Domain.Repositories
{
    public interface IExerciseRepository
    {
        Task<List<ExerciseDto>?> GetAsync(int pageNumber, int pageSize, string searchPhrase, bool idDescending, 
            int minRepeatQuantity, int maxRepeatQantity, int minSetQuantity, int maxSetQuantity, int reportId);
        Task UpdateAsync(Exercise exercise);
        Task DeleteAsync(int id);
        Task AddAsync(Exercise exercise);
    }
}
