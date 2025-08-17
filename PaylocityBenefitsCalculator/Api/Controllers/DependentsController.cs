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

    public DependentsController(IDependentQuery dependentQuery)
    {
        _dependentQuery = dependentQuery;
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
}
