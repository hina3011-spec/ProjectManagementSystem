using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ProjectManagement.API.DTOs;
using ProjectManagement.API.Models;
using ProjectManagement.API.Interfaces;
using FluentValidation;
namespace ProjectManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class EmployeesController : ControllerBase
    {
      
        private readonly IEmployeeService _employeeService;

        private readonly IValidator<EmployeeCreateDto> _employeeValidator;
        public EmployeesController(IEmployeeService employeeService,IValidator<EmployeeCreateDto> employeeValidator)
        {
            _employeeService = employeeService;
            _employeeValidator = employeeValidator;
        }
        // GET: api/Employees

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeResponseDto>>> GetEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();

            return Ok(employees);
        }

        // GET: api/Employees/5
       
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeResponseDto>> GetEmployeeById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            return Ok(employee);
        }
        // PUT: api/Employees/5
      //  [Authorize(Roles = "Admin,Manager")]
        [HttpPut("{id}")]
     
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeUpdateDto dto)
        {
            try
            {
                await _employeeService.UpdateEmployeeAsync(id, dto);

                return Ok("Employee updated successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
        // DELETE: api/Employees/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                await _employeeService.DeleteEmployeeAsync(id);

                return Ok("Employee deleted successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        // POST: api/Employees
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEmployee(EmployeeCreateDto dto)
        {
            try
            {
                // Call EmployeeService
                var validationResult = await _employeeValidator.ValidateAsync(dto);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors
                        .Select(e => e.ErrorMessage);

                    return BadRequest(errors);
                }
                var employeeId = await _employeeService.CreateEmployeeAsync(dto);

                return CreatedAtAction(
                    nameof(GetEmployeeById),
                    new { id = employeeId },
                    new
                    {
                        message = "Employee created successfully",
                        employeeId = employeeId
                    });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            throw new Exception("This is a test exception");
        }
        [HttpGet("paged")]
        public async Task<IActionResult> GetEmployeesPaged( [FromQuery] PaginationDto paginationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _employeeService
                .GetEmployeesPagedAsync(paginationDto);

            return Ok(result);
        }
    }
}