using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

[PrimaryKey("WorkflowId", "FromStatusId", "ToStatusId")]
public partial class WorkflowTransition
{
    [Key]
    public Guid WorkflowId { get; set; }

    [Key]
    public int FromStatusId { get; set; }

    [Key]
    public int ToStatusId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [ForeignKey("FromStatusId")]
    [InverseProperty("WorkflowTransitionFromStatuses")]
    public virtual Status FromStatus { get; set; } = null!;

    [ForeignKey("ToStatusId")]
    [InverseProperty("WorkflowTransitionToStatuses")]
    public virtual Status ToStatus { get; set; } = null!;

    [ForeignKey("WorkflowId")]
    [InverseProperty("WorkflowTransitions")]
    public virtual Workflow Workflow { get; set; } = null!;
}
