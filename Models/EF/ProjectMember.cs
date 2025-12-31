using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

[PrimaryKey("ProjectId", "UserId")]
public partial class ProjectMember
{
    [Key]
    public Guid ProjectId { get; set; }

    [Key]
    public Guid UserId { get; set; }

    [StringLength(50)]
    public string ProjectRole { get; set; } = null!;

    [Precision(3)]
    public DateTime AddedAt { get; set; }

    public bool IsActive { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("ProjectMembers")]
    public virtual Project Project { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("ProjectMembers")]
    public virtual User User { get; set; } = null!;
}
