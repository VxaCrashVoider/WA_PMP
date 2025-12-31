using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

public partial class User
{
    [Key]
    public Guid UserId { get; set; }

    [StringLength(320)]
    public string Email { get; set; } = null!;

    [StringLength(200)]
    public string DisplayName { get; set; } = null!;

    [MaxLength(256)]
    public byte[]? PasswordHash { get; set; }

    [StringLength(1000)]
    public string? AvatarUrl { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    [Precision(3)]
    public DateTime? LastLoginAt { get; set; }

    [Precision(3)]
    public DateTime CreatedAt { get; set; }

    [Precision(3)]
    public DateTime UpdatedAt { get; set; }

    public byte[] RowVer { get; set; } = null!;

    public bool IsSystemAdmin { get; set; }

    [InverseProperty("UploadedBy")]
    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    [InverseProperty("ActorUser")]
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    [InverseProperty("Assignee")]
    public virtual ICollection<Issue> IssueAssignees { get; set; } = new List<Issue>();

    [InverseProperty("Author")]
    public virtual ICollection<IssueComment> IssueComments { get; set; } = new List<IssueComment>();

    [InverseProperty("CreatedBy")]
    public virtual ICollection<IssueLink> IssueLinks { get; set; } = new List<IssueLink>();

    [InverseProperty("Reporter")]
    public virtual ICollection<Issue> IssueReporters { get; set; } = new List<Issue>();

    [InverseProperty("User")]
    public virtual ICollection<IssueWatcher> IssueWatchers { get; set; } = new List<IssueWatcher>();

    [InverseProperty("User")]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    [InverseProperty("User")]
    public virtual ICollection<OrganizationMember> OrganizationMembers { get; set; } = new List<OrganizationMember>();

    [InverseProperty("User")]
    public virtual ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();

    [InverseProperty("User")]
    public virtual ICollection<WorkLog> WorkLogs { get; set; } = new List<WorkLog>();
}
