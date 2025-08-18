using Api.Dtos.Paycheck;
using Api.Models;
using Api.Query;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PaychecksController : Controller
    {
        private readonly IPaycheckQuery _paycheckQuery;

        public PaychecksController(IPaycheckQuery paycheckQuery)
        {
            _paycheckQuery = paycheckQuery;
        }

        [SwaggerOperation(Summary = "Get employee paycheck by employee id")]
        [HttpGet("{employeeId}")]
        public async Task<ActionResult<ApiResponse<GetPaycheckDto>>> Get(int employeeId)
        {
            try
            {
                var paycheck = await _paycheckQuery.GetAllPaychecksByEmployeeIdAsync(employeeId);

                var response = new ApiResponse<GetPaycheckDto>
                {
                    Data = paycheck,
                    Success = true
                };

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<GetPaycheckDto>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<GetPaycheckDto>
                {
                    Success = false,
                    Error = $"An error occurred while processing your request: {ex.Message}"
                });
            }
        }
    }
}
