using ProjectManagement.API.DTOs;

namespace ProjectManagement.API.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync();

        Task<EmployeeResponseDto?> GetEmployeeByIdAsync(int id);
        Task<int> CreateEmployeeAsync(EmployeeCreateDto dto);
        Task UpdateEmployeeAsync(int id, EmployeeUpdateDto dto);

        Task DeleteEmployeeAsync(int id);
        Task<PagedResponseDto<EmployeeResponseDto>> GetEmployeesPagedAsync(PaginationDto paginationDto);
    }
}