using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using PersonalFinanceManagement.API.Controllers;
using PersonalFinanceManagement.API.Interfaces;
using PersonalFinanceManagement.API.Models;
using PersonalFinanceManagement.API.Models.DTO;
using PersonalFinanceManagement.API.Models.Enums;
using PersonalFinanceManagement.API.Profiles;
using PersonalFinanceManagement.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Tests;
public class RevenueControllerTests
{
    [Fact]
    public async Task ShouldReturnTheRevenueCreatedAsync()
    {
        //Arranje
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new RevenueProfile());
        });
        var mapper = mapperConfig.CreateMapper();
        var loggerService = Mock.Of<ILogger<IRevenueService>>();
        var loggerController = Mock.Of<ILogger<RevenueController>>();

        var revenue = CreateObjectRevenue();

        var createRevenueDTO = mapper.Map<CreateRevenueDTO>(revenue);
        var repoMock = new Mock<IRevenueRepository>();
        repoMock.Setup(r => r.AddAsync(createRevenueDTO)).ReturnsAsync(revenue.Id);

        var service = new RevenueService(loggerService, repoMock.Object, mapper);

        var revenueController = new RevenueController(loggerController, service);

        //Act
        var response = await revenueController.Create(createRevenueDTO);
        var result = response as CreatedAtActionResult;

        var revenueAdded = result?.Value as Revenue;
        //Assert
        Assert.Equal(201, result?.StatusCode);
        Assert.Equal(createRevenueDTO.Description, revenueAdded?.Description);
        Assert.Equal(createRevenueDTO.Date, revenueAdded?.Date);
        Assert.Equal(createRevenueDTO.Value, revenueAdded?.Value);
        Assert.Equal(createRevenueDTO.Type, revenueAdded?.Type);
        Assert.Equal(revenue.Id, revenueAdded?.Id);
    }
    
    [Fact]
    public async Task ShouldNotAddRevenueWithSameDescriptionAndSameMonthAsync()
    {
        //Arranje
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new RevenueProfile());
        });
        var mapper = mapperConfig.CreateMapper();
        var loggerService = Mock.Of<ILogger<IRevenueService>>();
        var loggerController = Mock.Of<ILogger<RevenueController>>();

        var revenue = CreateObjectRevenue();

        var repoMock = new Mock<IRevenueRepository>();
        repoMock
            .Setup(r => r.FindAsync(revenue.Description))
            .ReturnsAsync(new List<Revenue>() { revenue });

        var createRevenueDTO = mapper.Map<CreateRevenueDTO>(revenue);
        var service = new RevenueService(loggerService, repoMock.Object, mapper);

        var revenueController = new RevenueController(loggerController, service);

        //Act
        var response = await revenueController.Create(createRevenueDTO);
        var result = response as BadRequestObjectResult;

        var errorResponse = result?.Value as ErrorResponse;

        //Assert
        Assert.Equal("CreationError", errorResponse?.Error.Code);
        Assert.Equal(400, result?.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnRevenueAsync()
    {
        //Arranje
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new RevenueProfile());
        });
        var mapper = mapperConfig.CreateMapper();
        var loggerService = Mock.Of<ILogger<IRevenueService>>();
        var loggerController = Mock.Of<ILogger<RevenueController>>();

        var revenue = CreateObjectRevenue();

        var repoMock = new Mock<IRevenueRepository>();
        repoMock
            .Setup(r => r.FindAsync(revenue.Id))
            .ReturnsAsync(revenue);

        var service = new RevenueService(loggerService, repoMock.Object, mapper);
        var revenueController = new RevenueController(loggerController, service);

        //Act
        var response = await revenueController.Get(revenue.Id);
        var result = response as OkObjectResult;

        var returnedRevenue = result?.Value as Revenue;

        Assert.Equal(revenue.Id, returnedRevenue?.Id);
        Assert.Equal(200, result?.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnNotFoundAsync()
    {
        //Arranje
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new RevenueProfile());
        });
        var mapper = mapperConfig.CreateMapper();
        var loggerService = Mock.Of<ILogger<IRevenueService>>();
        var loggerController = Mock.Of<ILogger<RevenueController>>();

        var revenue = CreateObjectRevenue();

        var repoMock = new Mock<IRevenueRepository>();
        repoMock
            .Setup(r => r.FindAsync(revenue.Id));

        var service = new RevenueService(loggerService, repoMock.Object, mapper);
        var revenueController = new RevenueController(loggerController, service);

        //Act
        var response = await revenueController.Get(revenue.Id);
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
        var loggerService = Mock.Of<ILogger<IRevenueService>>();
        var loggerController = Mock.Of<ILogger<RevenueController>>();
        var mapper = Mock.Of<IMapper>();

        var repoMock = new Mock<IRevenueRepository>();
        repoMock
            .Setup(x => x.AllAsync())
            .ReturnsAsync(Enumerable.Empty<Revenue>());

        var service = new RevenueService(loggerService, repoMock.Object, mapper);
        var revenueController = new RevenueController(loggerController, service);

        //Act
        var response = await revenueController.Get();
        var result = response as NoContentResult;

        //Assert
        Assert.Equal(204, result?.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnEnumerableThatContainsRevenuesAsync()
    {
        //Arranje
        var loggerService = Mock.Of<ILogger<IRevenueService>>();
        var loggerController = Mock.Of<ILogger<RevenueController>>();
        var mapper = Mock.Of<IMapper>();

        var repoMock = new Mock<IRevenueRepository>();
        repoMock
            .Setup(x => x.AllAsync())
            .ReturnsAsync(new List<Revenue> { CreateObjectRevenue(), CreateObjectRevenue(id: 1), CreateObjectRevenue(id: 2)});

        var service = new RevenueService(loggerService, repoMock.Object, mapper);
        var revenueController = new RevenueController(loggerController, service);

        //Act
        var response = await revenueController.Get();
        var result = response as OkObjectResult;

        var allRevenues = result?.Value as IEnumerable<Revenue>;

        //Assert
        Assert.Equal(3, allRevenues?.Count());
        Assert.Equal(200, result?.StatusCode);
    }

    private Revenue CreateObjectRevenue(int id = 3, string description = "Salário", string dateTime = "2022-05-09", ExpenseRevenueType type = ExpenseRevenueType.Fixed, decimal value = 1000)
    {
        return new Revenue()
        {
            Id = id,
            Description = description,
            Date = DateTime.Parse(dateTime),
            Type = type,
            Value = value
        };
    }
}
