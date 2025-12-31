using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

[PrimaryKey("IssueId", "UserId")]
public partial class IssueWatcher
{
    [Key]
    public Guid IssueId { get; set; }

    [Key]
    public Guid UserId { get; set; }

    [Precision(3)]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("IssueId")]
    [InverseProperty("IssueWatchers")]
    public virtual Issue Issue { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("IssueWatchers")]
    public virtual User User { get; set; } = null!;
}
