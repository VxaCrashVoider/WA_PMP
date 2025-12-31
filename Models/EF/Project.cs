using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

public partial class Project
{
    [Key]
    public Guid ProjectId { get; set; }

    public Guid OrganizationId { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(20)]
    public string KeyCode { get; set; } = null!;

    public string? Description { get; set; }

    [StringLength(30)]
    public string ProjectType { get; set; } = null!;

    public bool IsArchived { get; set; }

    public bool IsDeleted { get; set; }

    [Precision(3)]
    public DateTime CreatedAt { get; set; }

    [Precision(3)]
    public DateTime UpdatedAt { get; set; }

    public byte[] RowVer { get; set; } = null!;

    [InverseProperty("Project")]
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    [InverseProperty("Project")]
    public virtual ICollection<Board> Boards { get; set; } = new List<Board>();

    [InverseProperty("Project")]
    public virtual ICollection<CustomField> CustomFields { get; set; } = new List<CustomField>();

    [InverseProperty("Project")]
    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();

    [InverseProperty("Project")]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    [ForeignKey("OrganizationId")]
    [InverseProperty("Projects")]
    public virtual Organization Organization { get; set; } = null!;

    [InverseProperty("Project")]
    public virtual ProjectCounter? ProjectCounter { get; set; }

    [InverseProperty("Project")]
    public virtual ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();

    [InverseProperty("Project")]
    public virtual ICollection<Workflow> Workflows { get; set; } = new List<Workflow>();
}
