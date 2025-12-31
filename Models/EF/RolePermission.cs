using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

[PrimaryKey("Scope", "RoleName", "PermissionKey")]
public partial class RolePermission
{
    [Key]
    [StringLength(20)]
    public string Scope { get; set; } = null!;

    [Key]
    [StringLength(50)]
    public string RoleName { get; set; } = null!;

    [Key]
    [StringLength(100)]
    public string PermissionKey { get; set; } = null!;

    [ForeignKey("PermissionKey")]
    [InverseProperty("RolePermissions")]
    public virtual Permission PermissionKeyNavigation { get; set; } = null!;
}
