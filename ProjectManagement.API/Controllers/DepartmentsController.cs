using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.API.Data;
using ProjectManagement.API.Models;

namespace ProjectManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ProjectManagementContext _context;

        public DepartmentsController(ProjectManagementContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            return await _context.Departments
                .AsNoTracking()
                .ToListAsync();
        }

        // GET: api/Departments/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
                return NotFound("Department not found");

            return department;
        }

        // POST: api/Departments
        [HttpPost]
        public async Task<ActionResult<Department>> CreateDepartment(Department department)
        {
            department.DepartmentId = 0;

            _context.Departments.Add(department);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetDepartment),
                new { id = department.DepartmentId },
                department);
        }

        // PUT: api/Departments/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, Department department)
        {
            if (id != department.DepartmentId)
                return BadRequest();

            var existingDepartment = await _context.Departments.FindAsync(id);

            if (existingDepartment == null)
                return NotFound("Department not found");

            existingDepartment.DepartmentName = department.DepartmentName;
            existingDepartment.Description = department.Description;
            existingDepartment.IsActive = department.IsActive;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Departments/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
                return NotFound("Department not found");

            _context.Departments.Remove(department);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict("Cannot delete department because employees are associated with it.");
            }

            return NoContent();
        }
    }
}