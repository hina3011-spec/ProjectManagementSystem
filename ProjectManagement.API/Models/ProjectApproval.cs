using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagement.API.Models;

public partial class ProjectApproval
{
    [Key]
    [Column("ApprovalID")]
    public int ApprovalId { get; set; }

    [Column("ProjectID")]
    public int ProjectId { get; set; }

    public int RequestedBy { get; set; }

    public int? ApprovedBy { get; set; }

    [StringLength(30)]
    public string? ApprovalStatus { get; set; }

    [StringLength(500)]
    public string? Remarks { get; set; }

    public DateTime? RequestedDate { get; set; }

    public DateTime? ApprovedDate { get; set; }

    [ForeignKey("ApprovedBy")]
    [InverseProperty("ProjectApprovalApprovedByNavigations")]
    public virtual Employee? ApprovedByNavigation { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("ProjectApprovals")]
    public virtual Project Project { get; set; } = null!;

    [ForeignKey("RequestedBy")]
    [InverseProperty("ProjectApprovalRequestedByNavigations")]
    public virtual Employee RequestedByNavigation { get; set; } = null!;
}
