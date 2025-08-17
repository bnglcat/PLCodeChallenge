using Api.Command;
using Api.Dtos.Dependent;
using Api.Models;
using Api.Query;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly IDependentQuery _dependentQuery;
    private readonly IAddDependent _addDependent;

    public DependentsController(IDependentQuery dependentQuery, IAddDependent addDependent)
    {
        _dependentQuery = dependentQuery;
        _addDependent = addDependent;
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var dependent = await _dependentQuery.GetDependentByIdAsync(id);
        if (dependent == null)
        {
            return NotFound(new ApiResponse<GetDependentDto>
            {
                Success = false,
                Message = "Dependent not found"
            });
        }
        var dependentDto = new GetDependentDto
        {
            Id = dependent.Id,
            FirstName = dependent.FirstName,
            LastName = dependent.LastName,
            Relationship = dependent.Relationship,
            DateOfBirth = dependent.DateOfBirth,
        };

        var response = new ApiResponse<GetDependentDto>
        {
            Data = dependentDto,
            Success = true
        };
        return Ok(response);

    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var dependents = await _dependentQuery.GetAllDependentsAsync();
        var dependentDtos = dependents.Select(d => new GetDependentDto
        {
            Id = d.Id,
            FirstName = d.FirstName,
            LastName = d.LastName,
            Relationship = d.Relationship,
            DateOfBirth = d.DateOfBirth
        }).ToList();
        var response = new ApiResponse<List<GetDependentDto>>
        {
            Data = dependentDtos,
            Success = true
        };
        return Ok(response);

    }

    [SwaggerOperation(Summary = "Add a dependant to an employee")]
    [HttpPost()]
    public async Task<ActionResult<ApiResponse<AddDependentDto>>> Add([FromBody] AddDependentDto dependent)
    {
        try
        {
            var newDependent = await _addDependent.AddDependentAsync(dependent);

            return Ok(new ApiResponse<AddDependentDto>
            {
                Data = new AddDependentDto
                {
                    Id = newDependent.Id,
                    FirstName = newDependent.FirstName,
                    LastName = newDependent.LastName,
                    Relationship = newDependent.Relationship,
                    DateOfBirth = newDependent.DateOfBirth,
                    EmployeeId = newDependent.EmployeeId
                },
                Success = true
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<AddDependentDto>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<AddDependentDto>
            {
                Success = false,
                Error = $"An error occurred while processing your request: {ex.Message}"
            });
        }        
    }
}
