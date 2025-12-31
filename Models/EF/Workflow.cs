using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

public partial class Workflow
{
    [Key]
    public Guid WorkflowId { get; set; }

    public Guid ProjectId { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = null!;

    public bool IsDefault { get; set; }

    [Precision(3)]
    public DateTime CreatedAt { get; set; }

    [Precision(3)]
    public DateTime UpdatedAt { get; set; }

    public byte[] RowVer { get; set; } = null!;

    [InverseProperty("Workflow")]
    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();

    [ForeignKey("ProjectId")]
    [InverseProperty("Workflows")]
    public virtual Project Project { get; set; } = null!;

    [InverseProperty("Workflow")]
    public virtual ICollection<WorkflowStatus> WorkflowStatuses { get; set; } = new List<WorkflowStatus>();

    [InverseProperty("Workflow")]
    public virtual ICollection<WorkflowTransition> WorkflowTransitions { get; set; } = new List<WorkflowTransition>();
}
