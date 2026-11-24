using Microsoft.EntityFrameworkCore;
using ProjectManagement.API.Data;
using ProjectManagement.API.Interfaces;
using ProjectManagement.API.Models;

namespace ProjectManagement.API.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ProjectManagementContext _context;

        public EmployeeRepository(ProjectManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
        }

        public async Task<Employee?> GetByEmailAsync(string email)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
}