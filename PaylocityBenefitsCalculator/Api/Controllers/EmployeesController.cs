using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Query;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeQuery _employeeQuery;

    public EmployeesController(IEmployeeQuery employeeQuery)
    {
        _employeeQuery = employeeQuery;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var employee = await _employeeQuery.GetEmployeeByIdAsync(id);

        if (employee == null)
        {
            return NotFound(new ApiResponse<GetEmployeeDto>
            {
                Success = false,
                Message = "Employee not found"
            });
        }

        var employeeDto = new GetEmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Salary = employee.Salary,
            DateOfBirth = employee.DateOfBirth,
            Dependents = [.. employee.Dependents.Select(d => new GetDependentDto
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Relationship = d.Relationship,
                DateOfBirth = d.DateOfBirth
            })]
        };

        var response = new ApiResponse<GetEmployeeDto>
        {
            Data = employeeDto,
            Success = true
        };

        return Ok(response);
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        var employees = await _employeeQuery.GetAllEmployeesAsync();

        var result = new ApiResponse<List<GetEmployeeDto>>
        {
            Data = [.. employees.Select(x=> new GetEmployeeDto()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Salary = x.Salary,
                DateOfBirth = x.DateOfBirth,
                Dependents = [.. x.Dependents.Select(d => new GetDependentDto
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Relationship = d.Relationship,
                    DateOfBirth = d.DateOfBirth
                })]
            }
              )],
            Success = true
        };

        return result;
    }


}
