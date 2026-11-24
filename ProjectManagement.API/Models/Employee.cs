using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagement.API.Models;

[Index("Email", Name = "UQ__Employee__A9D10534BA315D8B", IsUnique = true)]
public partial class Employee
{
    [Key]
    [Column("EmployeeID")]
    public int EmployeeId { get; set; }

    [StringLength(150)]
    public string EmployeeName { get; set; } = null!;

    [StringLength(150)]
    public string Email { get; set; } = null!;

    [Column("DepartmentID")]
    public int DepartmentId { get; set; }

    [StringLength(100)]
    public string? Designation { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    [ForeignKey("DepartmentId")]
    [InverseProperty("Employees")]
    public virtual Department Department { get; set; } = null!;

    [InverseProperty("ApprovedByNavigation")]
    public virtual ICollection<ProjectApproval> ProjectApprovalApprovedByNavigations { get; set; } = new List<ProjectApproval>();

    [InverseProperty("RequestedByNavigation")]
    public virtual ICollection<ProjectApproval> ProjectApprovalRequestedByNavigations { get; set; } = new List<ProjectApproval>();

    [InverseProperty("AssignedToEmployee")]
    public virtual ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();

    [InverseProperty("ProjectManager")]
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
