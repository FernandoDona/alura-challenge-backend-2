using PersonalFinanceManagement.API.Interfaces;
using PersonalFinanceManagement.API.Models;
using PersonalFinanceManagement.API.Models.DTO;

namespace PersonalFinanceManagement.API.Repositories;

public class RevenueRepository : IRevenueRepository
{
    private readonly ISqlDataAccess _dataAccess;

    public RevenueRepository(ISqlDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<int> AddAsync(CreateRevenueDTO createRevenueDTO)
    {
        return await _dataAccess.QueryFirstOrDefaultAsync<int, CreateRevenueDTO>("spRevenue_Insert", createRevenueDTO);
    }

    public async Task<IEnumerable<Revenue>> AllAsync()
    {
        return await _dataAccess.QueryAsync<Revenue, dynamic>("spRevenue_GetAll", new { });
    }

    public async Task<Revenue?> FindAsync(int id)
    {
        return await _dataAccess.QueryFirstOrDefaultAsync<Revenue?, dynamic>("spRevenue_Get", new { Id = id });
    }

    public async Task<IEnumerable<Revenue>> FindAsync(string description)
    {
        return await _dataAccess.QueryAsync<Revenue, dynamic>("spRevenue_Get", new { Description = description });
    }

    public async Task Remove(int id)
    {
        await _dataAccess.ExecuteAsync<dynamic>("spRevenue_Delete", new { Id = id });
    }

    public async Task Update(Revenue revenue)
    {
        await _dataAccess.ExecuteAsync<Revenue>("spRevenue_Update", revenue);
    }
}
