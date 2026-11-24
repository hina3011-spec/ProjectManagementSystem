using Microsoft.EntityFrameworkCore;
using ProjectManagement.API.Data;
using ProjectManagement.API.DTOs;
using ProjectManagement.API.Interfaces;
using ProjectManagement.API.Models;
using AutoMapper;

namespace ProjectManagement.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ProjectManagementContext _context;
        
        public EmployeeService(ProjectManagementContext context,ILogger<EmployeeService> logger,PasswordService passwordService, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _passwordService = passwordService;
            _mapper = mapper;
        }

        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;
        // Get all employees
        public async Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync()
        {
            var employees = await _context.Employees
                           .Include(e => e.Department)
                           .AsNoTracking()
                           .ToListAsync();

            return _mapper.Map<IEnumerable<EmployeeResponseDto>>(employees);

        }


        // Get employee by ID
        public async Task<EmployeeResponseDto?> GetEmployeeByIdAsync(int id)
        {
            //var employee = await (
            //    from e in _context.Employees
            //    join d in _context.Departments
            //        on e.DepartmentId equals d.DepartmentId
            //    where e.EmployeeId == id
            //    select new EmployeeResponseDto
            //    {
            //        EmployeeId = e.EmployeeId,
            //        EmployeeName = e.EmployeeName,
            //        Email = e.Email,
            //        Designation = e.Designation,
            //        DepartmentName = d.DepartmentName,
            //        IsActive = e.IsActive
            //    }
            //).AsNoTracking().FirstOrDefaultAsync();


            //return employee;
            var employee = await _context.Employees
        .Include(e => e.Department)
        .AsNoTracking()
        .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
                return null;

            return _mapper.Map<EmployeeResponseDto>(employee);
        }

        public async Task<int> CreateEmployeeAsync(EmployeeCreateDto dto)
        {

            _logger.LogInformation("Creating employee with email: {Email}",dto.Email);
            // 1. Check whether department exists
            var departmentExists = await _context.Departments
                .AnyAsync(d => d.DepartmentId == dto.DepartmentId);

            if (!departmentExists)
            {
                throw new ArgumentException("Invalid Department ID");
            }

            // 2. Check duplicate email
            var emailExists = await _context.Employees
                .AnyAsync(e => e.Email == dto.Email);

            if (emailExists)
            {
                throw new InvalidOperationException("Email already exists");
            }

            // 3. Create Employee object
            var employeeRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Employee");

            if (employeeRole == null)
            {
                throw new InvalidOperationException(
                    "Default Employee role does not exist.");
            }
            var employee = _mapper.Map<Employee>(dto);

            employee.RoleId = employeeRole.RoleId;

            employee.PasswordHash = _passwordService.HashPassword(
                employee,
                dto.Password);
            //var employee = new Employee()

            //{

            //    EmployeeName = dto.EmployeeName,
            //    Email = dto.Email,
            //    DepartmentId = dto.DepartmentId,
            //    Designation = dto.Designation,
            //    IsActive = true,
            //    RoleId = employeeRole.RoleId
            //};
            // Generate password hash
            //employee.PasswordHash = _passwordService.HashPassword(employee,dto.Password);


            // 4. Insert employee
            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Employee created successfully. Employee ID: {EmployeeId}", employee.EmployeeId);
            // 5. Return newly generated Employee ID
            return employee.EmployeeId;
        }

        public async Task UpdateEmployeeAsync(int id, EmployeeUpdateDto dto)
        {
            // 1. Find employee
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                throw new KeyNotFoundException("Employee not found");
            }

            // 2. Check department
            var departmentExists = await _context.Departments
                .AnyAsync(d => d.DepartmentId == dto.DepartmentId);

            if (!departmentExists)
            {
                throw new ArgumentException("Invalid Department ID");
            }

            // 3. Check duplicate email
            var emailExists = await _context.Employees
                .AnyAsync(e => e.Email == dto.Email && e.EmployeeId != id);

            if (emailExists)
            {
                throw new InvalidOperationException("Email already exists");
            }

            // 4. Update employee details
            //employee.EmployeeName = dto.EmployeeName;
            //employee.Email = dto.Email;
            //employee.DepartmentId = dto.DepartmentId;
            //employee.Designation = dto.Designation;
            //employee.IsActive = dto.IsActive;
            _mapper.Map(dto, employee);

            // 5. Save changes
            await _context.SaveChangesAsync();
        }


        public async Task DeleteEmployeeAsync(int id)
        {
            // 1. Find employee
            var employee = await _context.Employees.FindAsync(id);

            // 2. Check whether employee exists
            if (employee == null)
            {
                throw new KeyNotFoundException("Employee not found");
            }

            // 3. Delete employee
            _context.Employees.Remove(employee);

            // 4. Save changes
            await _context.SaveChangesAsync();
        }

        private readonly PasswordService _passwordService;

        public async Task<PagedResponseDto<EmployeeResponseDto>> GetEmployeesPagedAsync(PaginationDto paginationDto)
        {
            // 1. Calculate records to skip
            int skip = (paginationDto.PageNumber - 1)
                       * paginationDto.PageSize;

            // 2. Create employee query
            var query = _context.Employees
                .AsNoTracking()
                .Join(
                    _context.Departments,
                    e => e.DepartmentId,
                    d => d.DepartmentId,
                    (e, d) => new EmployeeResponseDto
                    {
                        EmployeeId = e.EmployeeId,
                        EmployeeName = e.EmployeeName,
                        Email = e.Email,
                        Designation = e.Designation,
                        DepartmentName = d.DepartmentName,
                        IsActive = e.IsActive

                    });
            // Apply search filter
            if (!string.IsNullOrWhiteSpace(paginationDto.Search))
            {
                query = query.Where(e =>
                    e.EmployeeName.Contains(paginationDto.Search) ||
                    e.Email.Contains(paginationDto.Search));
            }

            // 3. Get total employee count
            int totalRecords = await query.CountAsync();

            // 4. Get employees for requested page
            // Determine sorting order
            bool isDescending = paginationDto.SortOrder
                .Equals("desc", StringComparison.OrdinalIgnoreCase);

            // Apply dynamic sorting
            query = paginationDto.SortBy.ToLowerInvariant() switch
            {
                "employeename" => isDescending
                    ? query.OrderByDescending(e => e.EmployeeName)
                    : query.OrderBy(e => e.EmployeeName),

                "email" => isDescending
                    ? query.OrderByDescending(e => e.Email)
                    : query.OrderBy(e => e.Email),

                "departmentname" => isDescending
                    ? query.OrderByDescending(e => e.DepartmentName)
                    : query.OrderBy(e => e.DepartmentName),

                _ => isDescending
                    ? query.OrderByDescending(e => e.EmployeeId)
                    : query.OrderBy(e => e.EmployeeId)
            };

            // Apply pagination
            var employees = await query
                .Skip(skip)
                .Take(paginationDto.PageSize)
                .ToListAsync();

            // 5. Calculate total pages
            int totalPages = (int)Math.Ceiling(
                (double)totalRecords / paginationDto.PageSize);

            // 6. Return paginated response
            return new PagedResponseDto<EmployeeResponseDto>
            {
                PageNumber = paginationDto.PageNumber,
                PageSize = paginationDto.PageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                Data = employees
            };
        }
    }
}