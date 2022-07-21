using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PersonalFinanceManagement.API.Controllers;
using PersonalFinanceManagement.API.Interfaces;
using PersonalFinanceManagement.API.Models;
using PersonalFinanceManagement.API.Models.DTO;
using PersonalFinanceManagement.API.Models.Enums;
using PersonalFinanceManagement.API.Profiles;
using PersonalFinanceManagement.API.Services;

namespace PersonalFinanceManagement.Tests;
public class ExpenseControllerTests
{
    [Fact]
    public async Task ShouldReturnTheExpenseCreatedAsync()
    {
        //Arranje
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ExpenseProfile());
        });
        var mapper = mapperConfig.CreateMapper();
        var loggerService = Mock.Of<ILogger<IExpenseService>>();
        var loggerController = Mock.Of<ILogger<ExpenseController>>();

        var Expense = CreateObjectExpense();

        var createExpenseDTO = mapper.Map<CreateExpenseDTO>(Expense);
        var repoMock = new Mock<IExpenseRepository>();
        repoMock.Setup(r => r.AddAsync(createExpenseDTO)).ReturnsAsync(Expense.Id);

        var service = new ExpenseService(loggerService, repoMock.Object, mapper);

        var ExpenseController = new ExpenseController(loggerController, service);

        //Act
        var response = await ExpenseController.Create(createExpenseDTO);
        var result = response as CreatedAtActionResult;

        var ExpenseAdded = result?.Value as Expense;
        //Assert
        Assert.Equal(201, result?.StatusCode);
        Assert.Equal(createExpenseDTO.Description, ExpenseAdded?.Description);
        Assert.Equal(createExpenseDTO.Date, ExpenseAdded?.Date);
        Assert.Equal(createExpenseDTO.Value, ExpenseAdded?.Value);
        Assert.Equal(createExpenseDTO.Type, ExpenseAdded?.Type);
        Assert.Equal(Expense.Id, ExpenseAdded?.Id);
    }

    [Fact]
    public async Task ShouldNotAddExpenseWithSameDescriptionAndSameMonthAsync()
    {
        //Arranje
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ExpenseProfile());
        });
        var mapper = mapperConfig.CreateMapper();
        var loggerService = Mock.Of<ILogger<IExpenseService>>();
        var loggerController = Mock.Of<ILogger<ExpenseController>>();

        var Expense = CreateObjectExpense();

        var repoMock = new Mock<IExpenseRepository>();
        repoMock
            .Setup(r => r.FindAsync(Expense.Description))
            .ReturnsAsync(new List<Expense>() { Expense });

        var createExpenseDTO = mapper.Map<CreateExpenseDTO>(Expense);
        var service = new ExpenseService(loggerService, repoMock.Object, mapper);

        var ExpenseController = new ExpenseController(loggerController, service);

        //Act
        var response = await ExpenseController.Create(createExpenseDTO);
        var result = response as BadRequestObjectResult;

        var errorResponse = result?.Value as ErrorResponse;

        //Assert
        Assert.Equal("CreationError", errorResponse?.Error.Code);
        Assert.Equal(400, result?.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnExpenseAsync()
    {
        //Arranje
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ExpenseProfile());
        });
        var mapper = mapperConfig.CreateMapper();
        var loggerService = Mock.Of<ILogger<IExpenseService>>();
        var loggerController = Mock.Of<ILogger<ExpenseController>>();

        var Expense = CreateObjectExpense();

        var repoMock = new Mock<IExpenseRepository>();
        repoMock
            .Setup(r => r.FindAsync(Expense.Id))
            .ReturnsAsync(Expense);

        var service = new ExpenseService(loggerService, repoMock.Object, mapper);
        var ExpenseController = new ExpenseController(loggerController, service);

        //Act
        var response = await ExpenseController.Get(Expense.Id);
        var result = response as OkObjectResult;

        var returnedExpense = result?.Value as Expense;

        Assert.Equal(Expense.Id, returnedExpense?.Id);
        Assert.Equal(200, result?.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnNotFoundAsync()
    {
        //Arranje
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ExpenseProfile());
        });
        var mapper = mapperConfig.CreateMapper();
        var loggerService = Mock.Of<ILogger<IExpenseService>>();
        var loggerController = Mock.Of<ILogger<ExpenseController>>();

        var Expense = CreateObjectExpense();

        var repoMock = new Mock<IExpenseRepository>();
        repoMock
            .Setup(r => r.FindAsync(Expense.Id));

        var service = new ExpenseService(loggerService, repoMock.Object, mapper);
        var ExpenseController = new ExpenseController(loggerController, service);

        //Act
        var response = await ExpenseController.Get(Expense.Id);
        var result = response as NotFoundObjectResult;

        var errorResponse = result?.Value as ErrorResponse;

        //Assert
        Assert.Equal("NotFound", errorResponse?.Error.Code);
        Assert.Equal(404, result?.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnNoContentAsync()
    {
        //Arranje
        var loggerService = Mock.Of<ILogger<IExpenseService>>();
        var loggerController = Mock.Of<ILogger<ExpenseController>>();
        var mapper = Mock.Of<IMapper>();

        var repoMock = new Mock<IExpenseRepository>();
        repoMock
            .Setup(x => x.AllAsync())
            .ReturnsAsync(Enumerable.Empty<Expense>());

        var service = new ExpenseService(loggerService, repoMock.Object, mapper);
        var ExpenseController = new ExpenseController(loggerController, service);

        //Act
        var response = await ExpenseController.Get();
        var result = response as NoContentResult;

        //Assert
        Assert.Equal(204, result?.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnEnumerableThatContainsExpensesAsync()
    {
        //Arranje
        var loggerService = Mock.Of<ILogger<IExpenseService>>();
        var loggerController = Mock.Of<ILogger<ExpenseController>>();
        var mapper = Mock.Of<IMapper>();

        var repoMock = new Mock<IExpenseRepository>();
        repoMock
            .Setup(x => x.AllAsync())
            .ReturnsAsync(new List<Expense> { CreateObjectExpense(), CreateObjectExpense(id: 1), CreateObjectExpense(id: 2) });

        var service = new ExpenseService(loggerService, repoMock.Object, mapper);
        var ExpenseController = new ExpenseController(loggerController, service);

        //Act
        var response = await ExpenseController.Get();
        var result = response as OkObjectResult;

        var allExpenses = result?.Value as IEnumerable<Expense>;

        //Assert
        Assert.Equal(3, allExpenses?.Count());
        Assert.Equal(200, result?.StatusCode);
    }

    private Expense CreateObjectExpense(int id = 3, string description = "Salário", string dateTime = "2022-05-09", ExpenseRevenueType type = ExpenseRevenueType.Fixed, decimal value = 1000)
    {
        return new Expense()
        {
            Id = id,
            Description = description,
            Date = DateTime.Parse(dateTime),
            Type = type,
            Value = value
        };
    }
}
