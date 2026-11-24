using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagement.API.Models;

public partial class ProjectTask
{
    [Key]
    [Column("TaskID")]
    public int TaskId { get; set; }

    [Column("ProjectID")]
    public int ProjectId { get; set; }

    [StringLength(200)]
    public string TaskName { get; set; } = null!;

    [Column("AssignedToEmployeeID")]
    public int? AssignedToEmployeeId { get; set; }

    [StringLength(20)]
    public string? Priority { get; set; }

    [StringLength(30)]
    public string? Status { get; set; }

    public DateOnly? DueDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    [ForeignKey("AssignedToEmployeeId")]
    [InverseProperty("ProjectTasks")]
    public virtual Employee? AssignedToEmployee { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("ProjectTasks")]
    public virtual Project Project { get; set; } = null!;
}
