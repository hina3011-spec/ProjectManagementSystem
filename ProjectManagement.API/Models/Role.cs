namespace ProjectManagement.API.Models
{
    public class Role
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; } = string.Empty;

        public virtual ICollection<Employee> Employees { get; set; }
            = new List<Employee>();
    }
}