using AutoMapper;
using PersonalFinanceManagement.API.Interfaces;
using PersonalFinanceManagement.API.Models;
using PersonalFinanceManagement.API.Models.DTO;

namespace PersonalFinanceManagement.API.Services;

public class RevenueService : IRevenueService
{
    private readonly ILogger<IRevenueService> _logger;
    private readonly IRevenueRepository _repo;
    private readonly IMapper _mapper;

    public RevenueService(ILogger<IRevenueService> logger, IRevenueRepository repo, IMapper mapper)
    {
        _logger = logger;
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Revenue?> AddAsync(CreateRevenueDTO createRevenueDTO)
    {
        var revenuesInDb = await _repo.FindAsync(createRevenueDTO.Description);

        if (revenuesInDb.Any(r => r.Date.Month == createRevenueDTO.Date.Month))
        {
            return null;
        }

        var id = await _repo.AddAsync(createRevenueDTO);

        var revenue = _mapper.Map<Revenue>(createRevenueDTO);
        revenue.Id = id;

        return revenue;
    }

    public async Task<IEnumerable<Revenue>> GetAllAsync()
    {
        return await _repo.AllAsync();
    }

    public async Task<Revenue?> FindAsync(int id)
    {
        return await _repo.FindAsync(id);
    }

    public async Task Update(Revenue revenue)
    {
        await _repo.Update(revenue);
    }

    public async Task Remove(int id)
    {
        await _repo.Remove(id);
    }

    public async Task<IEnumerable<Revenue>> FindAsync(string description)
    {
        return await _repo.FindAsync(description);
    }
}
