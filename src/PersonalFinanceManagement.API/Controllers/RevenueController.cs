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
public class RevenueController : ControllerBase
{
    private readonly ILogger<RevenueController> _logger;
    private readonly IRevenueService _revenueService;

    public RevenueController(ILogger<RevenueController> logger, IRevenueService revenueService)
    {
        _logger = logger;
        _revenueService = revenueService;
    }

    [HttpPost(Name = "Create")]
    [SwaggerResponse(201, "The item was created", typeof(Revenue))]
    [SwaggerResponse(400, "Invalid request", typeof(ErrorResponse))]
    [SwaggerResponse(500, "An error ocurred", typeof(ErrorResponse))]
    public async Task<IActionResult> Create([FromBody][Required] CreateRevenueDTO createRevenueDTO)
    {
        try
        {
            var revenueAdded = await _revenueService.AddAsync(createRevenueDTO);

            if (revenueAdded is null)
            {
                var errorResponse = new ErrorResponse("CreationError", "This revenue already exists.");
                _logger.LogInformation("Attempt to insert an existing revenue.");
                return BadRequest(errorResponse);
            }

            return CreatedAtAction("Get", new { id = revenueAdded.Id}, revenueAdded);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ErrorResponse.From(ex));
        }
    }

    [HttpGet("{id}", Name = "Get")]
    [SwaggerResponse(200, "The object was found and returned", typeof(Revenue))]
    [SwaggerResponse(400, "Invalid request", typeof(ErrorResponse))]
    [SwaggerResponse(404, "Object not found", typeof(ErrorResponse))]
    [SwaggerResponse(500, "An error ocurred", typeof(ErrorResponse))]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var revenue = await _revenueService.FindAsync(id);

            if (revenue is null)
            {
                return NotFound(new ErrorResponse("NotFound", $"There is no Revenue with id = {id}"));
            }

            return Ok(revenue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ErrorResponse.From(ex));
        }
    }

    [HttpGet(Name = "GetByDesciption")]
    [SwaggerResponse(200, "The object was found and returned", typeof(Revenue))]
    [SwaggerResponse(204, "Doesnt exist any revenue", typeof(ErrorResponse))]
    [SwaggerResponse(400, "Invalid request", typeof(ErrorResponse))]
    [SwaggerResponse(500, "An error ocurred", typeof(ErrorResponse))]
    public async Task<IActionResult> Get([FromQuery] string description)
    {
        try
        {
            var revenue = await _revenueService.FindAsync(description);

            if (revenue.Any() == false)
            {
                return NoContent();
            }

            return Ok(revenue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ErrorResponse.From(ex));
        }
    }

    [HttpGet(Name = "GetAll")]
    [SwaggerResponse(200, "The object was found and returned", typeof(IEnumerable<Revenue>))]
    [SwaggerResponse(204, "Doesnt exist any revenue", typeof(ErrorResponse))]
    [SwaggerResponse(400, "Invalid request", typeof(ErrorResponse))]
    [SwaggerResponse(500, "An error ocurred", typeof(ErrorResponse))]
    public async Task<IActionResult> Get()
    {
        try
        {
            var revenue = await _revenueService.GetAllAsync();

            if (revenue.Any() == false)
            {
                return NoContent();
            }

            return Ok(revenue);
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
    public async Task<IActionResult> Update(Revenue revenue)
    {
        try
        {
            await _revenueService.Update(revenue);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ErrorResponse.From(ex));
        }
    }

    [HttpDelete("{id}", Name = "Delete")]
    [SwaggerResponse(200, "The object was sucessfully removed")]
    [SwaggerResponse(400, "Invalid request", typeof(ErrorResponse))]
    [SwaggerResponse(404, "Object not found", typeof(ErrorResponse))]
    [SwaggerResponse(500, "An error ocurred", typeof(ErrorResponse))]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _revenueService.Remove(id);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ErrorResponse.From(ex));
        }
    }
}
