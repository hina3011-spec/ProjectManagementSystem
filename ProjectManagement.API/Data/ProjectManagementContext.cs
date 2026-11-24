using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.API.Models;

namespace ProjectManagement.API.Data;

public partial class ProjectManagementContext : DbContext
{
    public ProjectManagementContext()
    {
    }

    public ProjectManagementContext(DbContextOptions<ProjectManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectApproval> ProjectApprovals { get; set; }

    public virtual DbSet<ProjectTask> ProjectTasks { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    ////#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=LAPTOP-J0RD9SB5\\MSSQLSERVER01;Database=ProjectManagementDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BCDD2772450");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF1803B2D24");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees_Departments");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__Projects__761ABED0AC69574A");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Pending");

            entity.HasOne(d => d.ProjectManager).WithMany(p => p.Projects).HasConstraintName("FK_Projects_Employees");
        });

        modelBuilder.Entity<ProjectApproval>(entity =>
        {
            entity.HasKey(e => e.ApprovalId).HasName("PK__ProjectA__328477D46B2F8790");

            entity.Property(e => e.ApprovalStatus).HasDefaultValue("Pending");
            entity.Property(e => e.RequestedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.ProjectApprovalApprovedByNavigations).HasConstraintName("FK_ProjectApprovals_Approver");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectApprovals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectApprovals_Projects");

            entity.HasOne(d => d.RequestedByNavigation).WithMany(p => p.ProjectApprovalRequestedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectApprovals_Requester");
        });

        modelBuilder.Entity<ProjectTask>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__ProjectT__7C6949D14126D58D");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Priority).HasDefaultValue("Medium");
            entity.Property(e => e.Status).HasDefaultValue("Pending");

            entity.HasOne(d => d.AssignedToEmployee).WithMany(p => p.ProjectTasks).HasConstraintName("FK_ProjectTasks_Employees");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectTasks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectTasks_Projects");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
