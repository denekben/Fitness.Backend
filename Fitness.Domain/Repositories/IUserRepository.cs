using Fitness.Shared.DTOs;

namespace Fitness.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<List<ProfileDto>?> GetCoachesAsync(int pageNumber, int pageSize, string searchPhrase, bool isDescending, string sex);
        Task<ProfileDto?> GetUserAsync(int id);
    }
}
