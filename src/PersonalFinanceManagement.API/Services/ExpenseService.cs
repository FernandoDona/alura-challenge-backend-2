using AutoMapper;
using PersonalFinanceManagement.API.Interfaces;
using PersonalFinanceManagement.API.Models;
using PersonalFinanceManagement.API.Models.DTO;

namespace PersonalFinanceManagement.API.Services;

public class ExpenseService : IExpenseService
{
    private readonly ILogger<IExpenseService> _logger;
    private readonly IExpenseRepository _repo;
    private readonly IMapper _mapper;

    public ExpenseService(ILogger<IExpenseService> logger, IExpenseRepository repo, IMapper mapper)
    {
        _logger = logger;
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Expense?> AddAsync(CreateExpenseDTO createExpenseDTO)
    {
        var ExpensesInDb = await _repo.FindAsync(createExpenseDTO.Description);

        if (ExpensesInDb.Any(r => r.Date.Month == createExpenseDTO.Date.Month))
        {
            return null;
        }

        var id = await _repo.AddAsync(createExpenseDTO);

        var Expense = _mapper.Map<Expense>(createExpenseDTO);
        Expense.Id = id;

        return Expense;
    }

    public async Task<IEnumerable<Expense>> GetAllAsync()
    {
        return await _repo.AllAsync();
    }

    public async Task<Expense?> FindAsync(int id)
    {
        return await _repo.FindAsync(id);
    }

    public async Task Update(Expense Expense)
    {
        await _repo.Update(Expense);
    }

    public async Task Remove(int id)
    {
        await _repo.Remove(id);
    }
}
