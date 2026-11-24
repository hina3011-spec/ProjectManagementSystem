namespace ProjectManagement.API.Models
{
    public partial class Employee
    {
        public string? PasswordHash { get; set; }
        public int? RoleId { get; set; }

        public virtual Role? Role { get; set; }
    }
}