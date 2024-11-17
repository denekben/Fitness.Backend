using Domain.Entities;
using Fitness.Shared.DTOs;

namespace Fitness.Domain.Repositories
{
    public interface IRequestRepository
    {
        Task<List<RequestDto>?> GetCurrentAthletesRequestsAsync(int id, int pageNumber, int pageSize, string searchPhrase, bool isDescending);
        Task<List<RequestDto>?> GetRequestsForCurrentCoachAsync(int id, int pageNumber, int pageSize, string searchPhrase, bool isDescending);
        Task AcceptRequest(int requestId);
        Task UpdateAsync(Request request);
        Task DeleteAsync(int id);
        Task AddAsync(Request request);
    }
}
