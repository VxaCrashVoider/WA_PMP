using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

[PrimaryKey("WorkflowId", "StatusId")]
public partial class WorkflowStatus
{
    [Key]
    public Guid WorkflowId { get; set; }

    [Key]
    public int StatusId { get; set; }

    public int SortOrder { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("WorkflowStatuses")]
    public virtual Status Status { get; set; } = null!;

    [ForeignKey("WorkflowId")]
    [InverseProperty("WorkflowStatuses")]
    public virtual Workflow Workflow { get; set; } = null!;
}
