using Domain.Entities;
using Fitness.Shared.DTOs;

namespace Fitness.Domain.Repositories
{
    public interface ITariffRepository
    {
        Task<List<TariffDto>?> GetAsync(int pageNumber, int pageSize, string searchPhrase, bool isDescending, decimal minPrice, decimal maxPrice);
        Task UpdateAsync(Tariff tariff);
        Task DeleteAsync(int id);
        Task AddAsync(Tariff tariff);
    }
}
