using PersonalFinanceManagement.API.Models;
using PersonalFinanceManagement.API.Models.DTO;
using System;
namespace PersonalFinanceManagement.API.Interfaces;

public interface IExpenseRepository
{
    Task<int> AddAsync(CreateExpenseDTO createExpenseDTO);
    Task<Expense?> FindAsync(int id);
    Task<IEnumerable<Expense>> FindAsync(string description);
    Task<IEnumerable<Expense>> AllAsync();
    Task Update(Expense Expense);
    Task Remove(int id);
}
