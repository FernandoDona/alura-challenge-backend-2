using PersonalFinanceManagement.API.Models.Enums;

namespace PersonalFinanceManagement.API.Models;

public class Expense
{
    public int Id { get; set; }
    public string Description { get; set; }
    public ExpenseRevenueType Type { get; set; }
    public decimal Value { get; set; }
    public DateTime Date { get; set; }
}
