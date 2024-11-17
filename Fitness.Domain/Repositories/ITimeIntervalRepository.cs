using Domain.Entities;
using Fitness.Shared.DTOs;

namespace Fitness.Domain.Repositories
{
    public interface ITimeIntervalRepository
    {
        Task<List<TimeIntervalDto>?> GetAsync(int currentUserId ,int pageNumber, int pageSize, bool isDescending);
        Task UpdateAsync(TimeInterval timeInterval);
        Task DeleteAsync(int id);
        Task AddAsync(TimeInterval timeInterval);
    }
}
