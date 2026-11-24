using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.API.DTOs
{
    public class EmployeeCreateDto
    {
        [Required(ErrorMessage = "Employee name is required")]
        [StringLength(150)]
        public string EmployeeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a department")]
        public int DepartmentId { get; set; }

        [StringLength(100)]
        public string? Designation { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}