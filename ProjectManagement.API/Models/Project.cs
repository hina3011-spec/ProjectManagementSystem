using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagement.API.Models;

[Index("ProjectCode", Name = "UQ__Projects__2F3A4948CE66C9C9", IsUnique = true)]
public partial class Project
{
    [Key]
    [Column("ProjectID")]
    public int ProjectId { get; set; }

    [StringLength(200)]
    public string ProjectName { get; set; } = null!;

    [StringLength(50)]
    public string ProjectCode { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Budget { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    [Column("ProjectManagerID")]
    public int? ProjectManagerId { get; set; }

    [StringLength(30)]
    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    [InverseProperty("Project")]
    public virtual ICollection<ProjectApproval> ProjectApprovals { get; set; } = new List<ProjectApproval>();

    [ForeignKey("ProjectManagerId")]
    [InverseProperty("Projects")]
    public virtual Employee? ProjectManager { get; set; }

    [InverseProperty("Project")]
    public virtual ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
}
