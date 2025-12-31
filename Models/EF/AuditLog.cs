using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

public partial class AuditLog
{
    [Key]
    public Guid AuditLogId { get; set; }

    public Guid? OrganizationId { get; set; }

    public Guid? ProjectId { get; set; }

    public Guid? ActorUserId { get; set; }

    [StringLength(50)]
    public string EntityType { get; set; } = null!;

    public Guid? EntityId { get; set; }

    [StringLength(50)]
    public string Action { get; set; } = null!;

    public string? DataJson { get; set; }

    [Precision(3)]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("ActorUserId")]
    [InverseProperty("AuditLogs")]
    public virtual User? ActorUser { get; set; }

    [ForeignKey("OrganizationId")]
    [InverseProperty("AuditLogs")]
    public virtual Organization? Organization { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("AuditLogs")]
    public virtual Project? Project { get; set; }
}
