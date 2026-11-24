namespace ProjectManagement.API.DTOs
{
    public class EmployeeResponseDto
    {
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Designation { get; set; }

        public string? DepartmentName { get; set; }

        public bool? IsActive { get; set; }
    }
}