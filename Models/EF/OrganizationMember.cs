using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

[PrimaryKey("OrganizationId", "UserId")]
public partial class OrganizationMember
{
    [Key]
    public Guid OrganizationId { get; set; }

    [Key]
    public Guid UserId { get; set; }

    [StringLength(50)]
    public string OrgRole { get; set; } = null!;

    [Precision(3)]
    public DateTime JoinedAt { get; set; }

    public bool IsActive { get; set; }

    [ForeignKey("OrganizationId")]
    [InverseProperty("OrganizationMembers")]
    public virtual Organization Organization { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("OrganizationMembers")]
    public virtual User User { get; set; } = null!;
}
