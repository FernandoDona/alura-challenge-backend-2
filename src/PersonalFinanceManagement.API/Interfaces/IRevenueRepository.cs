using PersonalFinanceManagement.API.Models;
using PersonalFinanceManagement.API.Models.DTO;

namespace PersonalFinanceManagement.API.Interfaces;

public interface IRevenueRepository
{
    Task<int> AddAsync(CreateRevenueDTO createRevenueDTO);
    Task<Revenue?> FindAsync(int id);
    Task<IEnumerable<Revenue>> FindAsync(string description);
    Task<IEnumerable<Revenue>> AllAsync();
    Task Update(Revenue revenue);
    Task Remove(int id);
}
