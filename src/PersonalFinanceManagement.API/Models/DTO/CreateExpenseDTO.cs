using PersonalFinanceManagement.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.API.Models.DTO;

public record CreateExpenseDTO (
    [Required] string Description,
    [Required] ExpenseRevenueType Type,
    [Required] decimal Value,
    [Required] DateTime Date
);
