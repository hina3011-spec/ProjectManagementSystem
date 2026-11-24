using Microsoft.EntityFrameworkCore;
using ProjectManagement.API.Models;

namespace ProjectManagement.API.Data
{
    public partial class ProjectManagementContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Role)
                .WithMany(r => r.Employees)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}