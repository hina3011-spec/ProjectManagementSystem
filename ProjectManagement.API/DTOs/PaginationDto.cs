using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.API.DTOs
{
    public class PaginationDto
    {
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }
        public string SortBy { get; set; } = "EmployeeId";

        public string SortOrder { get; set; } = "asc";
    }
}