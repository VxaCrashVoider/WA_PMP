using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

public partial class Label
{
    [Key]
    public Guid LabelId { get; set; }

    public Guid OrganizationId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Precision(3)]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("OrganizationId")]
    [InverseProperty("Labels")]
    public virtual Organization Organization { get; set; } = null!;

    [ForeignKey("LabelId")]
    [InverseProperty("Labels")]
    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();
}
