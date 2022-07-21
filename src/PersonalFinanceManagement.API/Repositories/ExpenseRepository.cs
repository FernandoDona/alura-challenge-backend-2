using PersonalFinanceManagement.API.Interfaces;
using PersonalFinanceManagement.API.Models;
using PersonalFinanceManagement.API.Models.DTO;

namespace PersonalFinanceManagement.API.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly ISqlDataAccess _dataAccess;

    public ExpenseRepository(ISqlDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<int> AddAsync(CreateExpenseDTO createExpenseDTO)
    {
        return await _dataAccess.QueryFirstOrDefaultAsync<int, CreateExpenseDTO>("spExpense_Insert", createExpenseDTO);
    }

    public async Task<IEnumerable<Expense>> AllAsync()
    {
        return await _dataAccess.QueryAsync<Expense, dynamic>("spExpense_GetAll", new { });
    }

    public async Task<Expense?> FindAsync(int id)
    {
        return await _dataAccess.QueryFirstOrDefaultAsync<Expense?, dynamic>("spExpense_Get", new { Id = id });
    }

    public async Task<IEnumerable<Expense>> FindAsync(string description)
    {
        return await _dataAccess.QueryAsync<Expense, dynamic>("spExpense_Get", new { Description = description });
    }

    public async Task Remove(int id)
    {
        await _dataAccess.ExecuteAsync<dynamic>("spExpense_Delete", new { Id = id });
    }

    public async Task Update(Expense Expense)
    {
        await _dataAccess.ExecuteAsync<Expense>("spExpense_Update", Expense);
    }
}
