using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.API.Interfaces;
using PersonalFinanceManagement.API.Models;
using PersonalFinanceManagement.API.Models.DTO;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ExpenseController : ControllerBase
{
    private readonly ILogger<ExpenseController> _logger;
    private readonly IExpenseService _ExpenseService;

    public ExpenseController(ILogger<ExpenseController> logger, IExpenseService ExpenseService)
    {
        _logger = logger;
        _ExpenseService = ExpenseService;
    }

    [HttpPost(Name = "Create")]
    [SwaggerResponse(201, "The item was created", typeof(Expense))]
    [SwaggerResponse(400, "Invalid request", typeof(ErrorResponse))]
    [SwaggerResponse(500, "An error ocurred", typeof(ErrorResponse))]
    public async Task<IActionResult> Create([FromBody][Required] CreateExpenseDTO createExpenseDTO)
    {
        try
        {
            var expenseAdded = await _ExpenseService.AddAsync(createExpenseDTO);

            if (expenseAdded is null)
            {
                var errorResponse = new ErrorResponse("CreationError", "This Expense already exists.");
                _logger.LogInformation("Attempt to insert an existing Expense.");
                return BadRequest(errorResponse);
            }

            return CreatedAtAction("Get", new { id = expenseAdded.Id}, expenseAdded);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ErrorResponse.From(ex));
        }
    }

    [HttpGet("{id}", Name = "Get")]
    [SwaggerResponse(200, "The object was found and returned", typeof(Expense))]
    [SwaggerResponse(400, "Invalid request", typeof(ErrorResponse))]
    [SwaggerResponse(404, "Object not found", typeof(ErrorResponse))]
    [SwaggerResponse(500, "An error ocurred", typeof(ErrorResponse))]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var expense = await _ExpenseService.FindAsync(id);

            if (expense is null)
            {
                return NotFound(new ErrorResponse("NotFound", $"There is no Expense with id = {id}"));
            }

            return Ok(expense);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ErrorResponse.From(ex));
        }
    }

    [HttpGet(Name = "GetByDesciption")]
    [SwaggerResponse(200, "The object was found and returned", typeof(Expense))]
    [SwaggerResponse(204, "Doesnt exist any Expense", typeof(ErrorResponse))]
    [SwaggerResponse(400, "Invalid request", typeof(ErrorResponse))]
    [SwaggerResponse(500, "An error ocurred", typeof(ErrorResponse))]
    public async Task<IActionResult> Get([FromQuery]string description)
    {
        try
        {
            var expense = await _ExpenseService.FindAsync(description);

            if (expense.Any() == false)
            {
                return NoContent();
            }

            return Ok(expense);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ErrorResponse.From(ex));
        }
    }

    [HttpGet(Name = "GetAll")]
    [SwaggerResponse(200, "The object was found and returned", typeof(IEnumerable<Expense>))]
    [SwaggerResponse(204, "Doesnt exist any Expense", typeof(ErrorResponse))]
    [SwaggerResponse(400, "Invalid request", typeof(ErrorResponse))]
    [SwaggerResponse(500, "An error ocurred", typeof(ErrorResponse))]
    public async Task<IActionResult> Get()
    {
        try
        {
            var expense = await _ExpenseService.GetAllAsync();

            if (expense.Any() == false)
            {
                return NoContent();
            }

            return Ok(expense);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ErrorResponse.From(ex));
        }
    }

    [HttpPut(Name = "Update")]
    [SwaggerResponse(200, "The object was sucessfully updated")]
    [SwaggerResponse(400, "Invalid request", typeof(ErrorResponse))]
    [SwaggerResponse(404, "Object not found", typeof(ErrorResponse))]
    [SwaggerResponse(500, "An error ocurred", typeof(ErrorResponse))]
    public async Task<IActionResult> Update(Expense Expense)
    {
        try
        {
            await _ExpenseService.Update(Expense);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ErrorResponse.From(ex));
        }
    }

    [HttpDelete("{id}", Name = "Delete")]
    [SwaggerResponse(204, "The object was sucessfully removed")]
    [SwaggerResponse(400, "Invalid request", typeof(ErrorResponse))]
    [SwaggerResponse(404, "Object not found", typeof(ErrorResponse))]
    [SwaggerResponse(500, "An error ocurred", typeof(ErrorResponse))]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _ExpenseService.Remove(id);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ErrorResponse.From(ex));
        }
    }
}
