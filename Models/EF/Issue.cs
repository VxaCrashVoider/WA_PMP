using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

public partial class Issue
{
    [Key]
    public Guid IssueId { get; set; }

    public Guid ProjectId { get; set; }

    public int IssueNo { get; set; }

    [StringLength(20)]
    public string ProjectKeySnapshot { get; set; } = null!;

    [StringLength(41)]
    public string IssueKey { get; set; } = null!;

    public int IssueTypeId { get; set; }

    public int PriorityId { get; set; }

    public int StatusId { get; set; }

    public Guid? WorkflowId { get; set; }

    [StringLength(500)]
    public string Summary { get; set; } = null!;

    public string? Description { get; set; }

    public Guid ReporterId { get; set; }

    public Guid? AssigneeId { get; set; }

    public Guid? ParentIssueId { get; set; }

    public Guid? EpicIssueId { get; set; }

    public Guid? BoardId { get; set; }

    public Guid? SprintId { get; set; }

    public long Rank { get; set; }

    public int? OriginalEstimateMin { get; set; }

    public int? RemainingEstimateMin { get; set; }

    public int TimeSpentMin { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? StoryPoints { get; set; }

    public DateOnly? DueDate { get; set; }

    [StringLength(50)]
    public string? Resolution { get; set; }

    [Precision(3)]
    public DateTime? ResolvedAt { get; set; }

    public bool IsDeleted { get; set; }

    [Precision(3)]
    public DateTime CreatedAt { get; set; }

    [Precision(3)]
    public DateTime UpdatedAt { get; set; }

    public byte[] RowVer { get; set; } = null!;

    [ForeignKey("AssigneeId")]
    [InverseProperty("IssueAssignees")]
    public virtual User? Assignee { get; set; }

    [InverseProperty("Issue")]
    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    [ForeignKey("BoardId")]
    [InverseProperty("Issues")]
    public virtual Board? Board { get; set; }

    [ForeignKey("EpicIssueId")]
    [InverseProperty("InverseEpicIssue")]
    public virtual Issue? EpicIssue { get; set; }

    [InverseProperty("EpicIssue")]
    public virtual ICollection<Issue> InverseEpicIssue { get; set; } = new List<Issue>();

    [InverseProperty("ParentIssue")]
    public virtual ICollection<Issue> InverseParentIssue { get; set; } = new List<Issue>();

    [InverseProperty("Issue")]
    public virtual ICollection<IssueComment> IssueComments { get; set; } = new List<IssueComment>();

    [InverseProperty("Issue")]
    public virtual ICollection<IssueCustomFieldValue> IssueCustomFieldValues { get; set; } = new List<IssueCustomFieldValue>();

    [InverseProperty("FromIssue")]
    public virtual ICollection<IssueLink> IssueLinkFromIssues { get; set; } = new List<IssueLink>();

    [InverseProperty("ToIssue")]
    public virtual ICollection<IssueLink> IssueLinkToIssues { get; set; } = new List<IssueLink>();

    [ForeignKey("IssueTypeId")]
    [InverseProperty("Issues")]
    public virtual IssueType IssueType { get; set; } = null!;

    [InverseProperty("Issue")]
    public virtual ICollection<IssueWatcher> IssueWatchers { get; set; } = new List<IssueWatcher>();

    [InverseProperty("Issue")]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    [ForeignKey("ParentIssueId")]
    [InverseProperty("InverseParentIssue")]
    public virtual Issue? ParentIssue { get; set; }

    [ForeignKey("PriorityId")]
    [InverseProperty("Issues")]
    public virtual Priority Priority { get; set; } = null!;

    [ForeignKey("ProjectId")]
    [InverseProperty("Issues")]
    public virtual Project Project { get; set; } = null!;

    [ForeignKey("ReporterId")]
    [InverseProperty("IssueReporters")]
    public virtual User Reporter { get; set; } = null!;

    [ForeignKey("SprintId")]
    [InverseProperty("Issues")]
    public virtual Sprint? Sprint { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("Issues")]
    public virtual Status Status { get; set; } = null!;

    [InverseProperty("Issue")]
    public virtual ICollection<WorkLog> WorkLogs { get; set; } = new List<WorkLog>();

    [ForeignKey("WorkflowId")]
    [InverseProperty("Issues")]
    public virtual Workflow? Workflow { get; set; }

    [ForeignKey("IssueId")]
    [InverseProperty("Issues")]
    public virtual ICollection<Label> Labels { get; set; } = new List<Label>();
}
