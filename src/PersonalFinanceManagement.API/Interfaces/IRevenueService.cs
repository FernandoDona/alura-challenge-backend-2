using PersonalFinanceManagement.API.Models;
using PersonalFinanceManagement.API.Models.DTO;

namespace PersonalFinanceManagement.API.Interfaces;

public interface IRevenueService
{
    Task<Revenue?> AddAsync(CreateRevenueDTO createRevenueDTO);
    Task<Revenue?> FindAsync(int id);
    Task<IEnumerable<Revenue>> FindAsync(string description);
    Task<IEnumerable<Revenue>> GetAllAsync();
    Task Update(Revenue revenue);
    Task Remove(int id);
}
