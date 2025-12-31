using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

public partial class IssueLink
{
    [Key]
    public Guid IssueLinkId { get; set; }

    public Guid FromIssueId { get; set; }

    public Guid ToIssueId { get; set; }

    [StringLength(50)]
    public string LinkType { get; set; } = null!;

    public Guid CreatedById { get; set; }

    [Precision(3)]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("CreatedById")]
    [InverseProperty("IssueLinks")]
    public virtual User CreatedBy { get; set; } = null!;

    [ForeignKey("FromIssueId")]
    [InverseProperty("IssueLinkFromIssues")]
    public virtual Issue FromIssue { get; set; } = null!;

    [ForeignKey("ToIssueId")]
    [InverseProperty("IssueLinkToIssues")]
    public virtual Issue ToIssue { get; set; } = null!;
}
