using AutoMapper;
using PersonalFinanceManagement.API.Models;
using PersonalFinanceManagement.API.Models.DTO;

namespace PersonalFinanceManagement.API.Profiles;

public class ExpenseProfile : Profile
{
    public ExpenseProfile()
    {
        CreateMap<CreateExpenseDTO, Expense>();
        CreateMap<Expense, CreateExpenseDTO>();
    }
}
