using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

public partial class Sprint
{
    [Key]
    public Guid SprintId { get; set; }

    public Guid BoardId { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(1000)]
    public string? Goal { get; set; }

    [StringLength(20)]
    public string State { get; set; } = null!;

    [Precision(3)]
    public DateTime? StartDate { get; set; }

    [Precision(3)]
    public DateTime? EndDate { get; set; }

    [Precision(3)]
    public DateTime? CompleteDate { get; set; }

    [Precision(3)]
    public DateTime CreatedAt { get; set; }

    [Precision(3)]
    public DateTime UpdatedAt { get; set; }

    public byte[] RowVer { get; set; } = null!;

    [ForeignKey("BoardId")]
    [InverseProperty("Sprints")]
    public virtual Board Board { get; set; } = null!;

    [InverseProperty("Sprint")]
    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();
}
