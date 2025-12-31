using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

public partial class WorkLog
{
    [Key]
    public Guid WorkLogId { get; set; }

    public Guid IssueId { get; set; }

    public Guid UserId { get; set; }

    public int TimeSpentMin { get; set; }

    [Precision(3)]
    public DateTime StartedAt { get; set; }

    [StringLength(1000)]
    public string? Comment { get; set; }

    [Precision(3)]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("IssueId")]
    [InverseProperty("WorkLogs")]
    public virtual Issue Issue { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("WorkLogs")]
    public virtual User User { get; set; } = null!;
}
