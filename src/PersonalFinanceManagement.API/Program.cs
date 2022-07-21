using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.API;
using PersonalFinanceManagement.API.Filters;
using PersonalFinanceManagement.API.Interfaces;
using PersonalFinanceManagement.API.Models;
using PersonalFinanceManagement.API.Repositories;
using PersonalFinanceManagement.API.Services;
using Serilog;
using System.Text.Json.Serialization;

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

    builder.Host.UseSerilog();

    // Add services to the container.
    builder.Services
        .AddControllers(o => o.Filters.Add(typeof(ErrorResponseFilter)))
        .ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                return new BadRequestObjectResult(ErrorResponse.FromModelState(context.ModelState));
            };
        });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c => 
    { 
        c.EnableAnnotations(); 
        c.SchemaFilter<EnumSchemaFilter>();
    });
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
    builder.Services.AddSingleton<IRevenueRepository, RevenueRepository>();
    builder.Services.AddSingleton<IRevenueService, RevenueService>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The application terminated unexpectedly.");
}
finally
{
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}