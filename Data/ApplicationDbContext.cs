using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WA_ProjectManagement.Models.EF;

namespace WA_ProjectManagement.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Board> Boards { get; set; }

    public virtual DbSet<CustomField> CustomFields { get; set; }

    public virtual DbSet<CustomFieldOption> CustomFieldOptions { get; set; }

    public virtual DbSet<Issue> Issues { get; set; }

    public virtual DbSet<IssueComment> IssueComments { get; set; }

    public virtual DbSet<IssueCustomFieldValue> IssueCustomFieldValues { get; set; }

    public virtual DbSet<IssueLink> IssueLinks { get; set; }

    public virtual DbSet<IssueType> IssueTypes { get; set; }

    public virtual DbSet<IssueWatcher> IssueWatchers { get; set; }

    public virtual DbSet<Label> Labels { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<OrganizationMember> OrganizationMembers { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Priority> Priorities { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectCounter> ProjectCounters { get; set; }

    public virtual DbSet<ProjectMember> ProjectMembers { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<Sprint> Sprints { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WorkLog> WorkLogs { get; set; }

    public virtual DbSet<Workflow> Workflows { get; set; }

    public virtual DbSet<WorkflowStatus> WorkflowStatuses { get; set; }

    public virtual DbSet<WorkflowTransition> WorkflowTransitions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Fallback connection string for local development only.
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=WA_PM;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.Property(e => e.AttachmentId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Attachments_CreatedAt");

            entity.HasOne(d => d.Issue).WithMany(p => p.Attachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attachments_Issue");

            entity.HasOne(d => d.UploadedBy).WithMany(p => p.Attachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attachments_User");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.Property(e => e.AuditLogId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_AuditLogs_CreatedAt");

            entity.HasOne(d => d.ActorUser).WithMany(p => p.AuditLogs).HasConstraintName("FK_AuditLogs_Actor");

            entity.HasOne(d => d.Organization).WithMany(p => p.AuditLogs).HasConstraintName("FK_AuditLogs_Org");

            entity.HasOne(d => d.Project).WithMany(p => p.AuditLogs).HasConstraintName("FK_AuditLogs_Project");
        });

        modelBuilder.Entity<Board>(entity =>
        {
            entity.Property(e => e.BoardId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Boards_CreatedAt");
            entity.Property(e => e.RowVer)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Boards_UpdatedAt");

            entity.HasOne(d => d.Project).WithMany(p => p.Boards)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Boards_Project");
        });

        modelBuilder.Entity<CustomField>(entity =>
        {
            entity.Property(e => e.CustomFieldId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_CustomFields_CreatedAt");

            entity.HasOne(d => d.Project).WithMany(p => p.CustomFields)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomFields_Project");
        });

        modelBuilder.Entity<CustomFieldOption>(entity =>
        {
            entity.Property(e => e.OptionId).ValueGeneratedNever();

            entity.HasOne(d => d.CustomField).WithMany(p => p.CustomFieldOptions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomFieldOptions_Field");
        });

        modelBuilder.Entity<Issue>(entity =>
        {
            entity.Property(e => e.IssueId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Issues_CreatedAt");
            entity.Property(e => e.IssueKey).HasComputedColumnSql("(concat([ProjectKeySnapshot],'-',CONVERT([varchar](20),[IssueNo])))", true);
            entity.Property(e => e.RowVer)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Issues_UpdatedAt");

            entity.HasOne(d => d.Assignee).WithMany(p => p.IssueAssignees).HasConstraintName("FK_Issues_Assignee");

            entity.HasOne(d => d.Board).WithMany(p => p.Issues).HasConstraintName("FK_Issues_Board");

            entity.HasOne(d => d.EpicIssue).WithMany(p => p.InverseEpicIssue).HasConstraintName("FK_Issues_Epic");

            entity.HasOne(d => d.IssueType).WithMany(p => p.Issues)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Issues_IssueType");

            entity.HasOne(d => d.ParentIssue).WithMany(p => p.InverseParentIssue).HasConstraintName("FK_Issues_Parent");

            entity.HasOne(d => d.Priority).WithMany(p => p.Issues)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Issues_Priority");

            entity.HasOne(d => d.Project).WithMany(p => p.Issues)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Issues_Project");

            entity.HasOne(d => d.Reporter).WithMany(p => p.IssueReporters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Issues_Reporter");

            entity.HasOne(d => d.Sprint).WithMany(p => p.Issues).HasConstraintName("FK_Issues_Sprint");

            entity.HasOne(d => d.Status).WithMany(p => p.Issues)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Issues_Status");

            entity.HasOne(d => d.Workflow).WithMany(p => p.Issues).HasConstraintName("FK_Issues_Workflow");

            entity.HasMany(d => d.Labels).WithMany(p => p.Issues)
                .UsingEntity<Dictionary<string, object>>(
                    "IssueLabel",
                    r => r.HasOne<Label>().WithMany()
                        .HasForeignKey("LabelId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_IssueLabels_Label"),
                    l => l.HasOne<Issue>().WithMany()
                        .HasForeignKey("IssueId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_IssueLabels_Issue"),
                    j =>
                    {
                        j.HasKey("IssueId", "LabelId");
                        j.ToTable("IssueLabels");
                    });
        });

        modelBuilder.Entity<IssueComment>(entity =>
        {
            entity.Property(e => e.CommentId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_IssueComments_CreatedAt");
            entity.Property(e => e.RowVer)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_IssueComments_UpdatedAt");

            entity.HasOne(d => d.Author).WithMany(p => p.IssueComments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IssueComments_Author");

            entity.HasOne(d => d.Issue).WithMany(p => p.IssueComments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IssueComments_Issue");
        });

        modelBuilder.Entity<IssueCustomFieldValue>(entity =>
        {
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_IssueCFV_UpdatedAt");

            entity.HasOne(d => d.CustomField).WithMany(p => p.IssueCustomFieldValues)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IssueCFV_Field");

            entity.HasOne(d => d.Issue).WithMany(p => p.IssueCustomFieldValues)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IssueCFV_Issue");

            entity.HasOne(d => d.ValueOption).WithMany(p => p.IssueCustomFieldValues).HasConstraintName("FK_IssueCFV_Option");
        });

        modelBuilder.Entity<IssueLink>(entity =>
        {
            entity.Property(e => e.IssueLinkId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_IssueLinks_CreatedAt");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.IssueLinks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IssueLinks_User");

            entity.HasOne(d => d.FromIssue).WithMany(p => p.IssueLinkFromIssues)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IssueLinks_From");

            entity.HasOne(d => d.ToIssue).WithMany(p => p.IssueLinkToIssues)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IssueLinks_To");
        });

        modelBuilder.Entity<IssueWatcher>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_IssueWatchers_CreatedAt");

            entity.HasOne(d => d.Issue).WithMany(p => p.IssueWatchers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IssueWatchers_Issue");

            entity.HasOne(d => d.User).WithMany(p => p.IssueWatchers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IssueWatchers_User");
        });

        modelBuilder.Entity<Label>(entity =>
        {
            entity.Property(e => e.LabelId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Labels_CreatedAt");

            entity.HasOne(d => d.Organization).WithMany(p => p.Labels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Labels_Org");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.Property(e => e.NotificationId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Notifications_CreatedAt");

            entity.HasOne(d => d.Issue).WithMany(p => p.Notifications).HasConstraintName("FK_Notifications_Issue");

            entity.HasOne(d => d.Organization).WithMany(p => p.Notifications).HasConstraintName("FK_Notifications_Org");

            entity.HasOne(d => d.Project).WithMany(p => p.Notifications).HasConstraintName("FK_Notifications_Project");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notifications_User");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.Property(e => e.OrganizationId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Organizations_CreatedAt");
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF_Organizations_IsActive");
            entity.Property(e => e.Plan).HasDefaultValue("Free", "DF_Organizations_Plan");
            entity.Property(e => e.RowVer)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Organizations_UpdatedAt");
        });

        modelBuilder.Entity<OrganizationMember>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF_OrgMembers_IsActive");
            entity.Property(e => e.JoinedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_OrgMembers_JoinedAt");
            entity.Property(e => e.OrgRole).HasDefaultValue("Member", "DF_OrgMembers_Role");

            entity.HasOne(d => d.Organization).WithMany(p => p.OrganizationMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrgMembers_Org");

            entity.HasOne(d => d.User).WithMany(p => p.OrganizationMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrgMembers_User");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.Property(e => e.ProjectId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Projects_CreatedAt");
            entity.Property(e => e.ProjectType).HasDefaultValue("Software", "DF_Projects_Type");
            entity.Property(e => e.RowVer)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Projects_UpdatedAt");

            entity.HasOne(d => d.Organization).WithMany(p => p.Projects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Projects_Org");
        });

        modelBuilder.Entity<ProjectCounter>(entity =>
        {
            entity.Property(e => e.ProjectId).ValueGeneratedNever();
            entity.Property(e => e.RowVer)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.Project).WithOne(p => p.ProjectCounter)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectCounters_Project");
        });

        modelBuilder.Entity<ProjectMember>(entity =>
        {
            entity.Property(e => e.AddedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_ProjectMembers_AddedAt");
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF_ProjectMembers_IsActive");
            entity.Property(e => e.ProjectRole).HasDefaultValue("Developer", "DF_ProjectMembers_Role");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectMembers_Project");

            entity.HasOne(d => d.User).WithMany(p => p.ProjectMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectMembers_User");
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasOne(d => d.PermissionKeyNavigation).WithMany(p => p.RolePermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermissions_Permission");
        });

        modelBuilder.Entity<Sprint>(entity =>
        {
            entity.Property(e => e.SprintId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Sprints_CreatedAt");
            entity.Property(e => e.RowVer)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.State).HasDefaultValue("Planned", "DF_Sprints_State");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Sprints_UpdatedAt");

            entity.HasOne(d => d.Board).WithMany(p => p.Sprints)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sprints_Board");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Users_CreatedAt");
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF_Users_IsActive");
            entity.Property(e => e.RowVer)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Users_UpdatedAt");
        });

        modelBuilder.Entity<WorkLog>(entity =>
        {
            entity.Property(e => e.WorkLogId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_WorkLogs_CreatedAt");

            entity.HasOne(d => d.Issue).WithMany(p => p.WorkLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkLogs_Issue");

            entity.HasOne(d => d.User).WithMany(p => p.WorkLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkLogs_User");
        });

        modelBuilder.Entity<Workflow>(entity =>
        {
            entity.Property(e => e.WorkflowId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Workflows_CreatedAt");
            entity.Property(e => e.IsDefault).HasDefaultValue(true, "DF_Workflows_IsDefault");
            entity.Property(e => e.RowVer)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Workflows_UpdatedAt");

            entity.HasOne(d => d.Project).WithMany(p => p.Workflows)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Workflows_Project");
        });

        modelBuilder.Entity<WorkflowStatus>(entity =>
        {
            entity.HasOne(d => d.Status).WithMany(p => p.WorkflowStatuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkflowStatuses_Status");

            entity.HasOne(d => d.Workflow).WithMany(p => p.WorkflowStatuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkflowStatuses_Workflow");
        });

        modelBuilder.Entity<WorkflowTransition>(entity =>
        {
            entity.HasOne(d => d.FromStatus).WithMany(p => p.WorkflowTransitionFromStatuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkflowTransitions_From");

            entity.HasOne(d => d.ToStatus).WithMany(p => p.WorkflowTransitionToStatuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkflowTransitions_To");

            entity.HasOne(d => d.Workflow).WithMany(p => p.WorkflowTransitions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkflowTransitions_Workflow");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
