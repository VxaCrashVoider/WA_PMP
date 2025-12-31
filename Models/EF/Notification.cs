using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

public partial class Notification
{
    [Key]
    public Guid NotificationId { get; set; }

    public Guid UserId { get; set; }

    public Guid? OrganizationId { get; set; }

    public Guid? ProjectId { get; set; }

    public Guid? IssueId { get; set; }

    [StringLength(50)]
    public string Type { get; set; } = null!;

    public string PayloadJson { get; set; } = null!;

    public bool IsRead { get; set; }

    [Precision(3)]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("IssueId")]
    [InverseProperty("Notifications")]
    public virtual Issue? Issue { get; set; }

    [ForeignKey("OrganizationId")]
    [InverseProperty("Notifications")]
    public virtual Organization? Organization { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("Notifications")]
    public virtual Project? Project { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Notifications")]
    public virtual User User { get; set; } = null!;
}
