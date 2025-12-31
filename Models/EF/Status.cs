using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

public partial class Status
{
    [Key]
    public int StatusId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(20)]
    public string Category { get; set; } = null!;

    public int SortOrder { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();

    [InverseProperty("Status")]
    public virtual ICollection<WorkflowStatus> WorkflowStatuses { get; set; } = new List<WorkflowStatus>();

    [InverseProperty("FromStatus")]
    public virtual ICollection<WorkflowTransition> WorkflowTransitionFromStatuses { get; set; } = new List<WorkflowTransition>();

    [InverseProperty("ToStatus")]
    public virtual ICollection<WorkflowTransition> WorkflowTransitionToStatuses { get; set; } = new List<WorkflowTransition>();
}
