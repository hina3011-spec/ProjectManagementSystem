using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.API.DTOs
{
    public class EmployeeUpdateDto
    {
        [Required(ErrorMessage = "Employee name is required")]
        [StringLength(150)]
        public string EmployeeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int DepartmentId { get; set; }

        [StringLength(100)]
        public string? Designation { get; set; }

        public bool IsActive { get; set; }
    }
}