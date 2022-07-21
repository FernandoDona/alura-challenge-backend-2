using PersonalFinanceManagement.API.Models;
using PersonalFinanceManagement.API.Models.DTO;

namespace PersonalFinanceManagement.API.Interfaces;

public interface IExpenseService
{
    Task<Expense?> AddAsync(CreateExpenseDTO createExpenseDTO);
    Task<Expense?> FindAsync(int id);
    Task<IEnumerable<Expense>> FindAsync(string description);
    Task<IEnumerable<Expense>> GetAllAsync();
    Task Update(Expense Expense);
    Task Remove(int id);
}
