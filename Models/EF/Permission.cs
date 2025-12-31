using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

public partial class Permission
{
    [Key]
    [StringLength(100)]
    public string PermissionKey { get; set; } = null!;

    [StringLength(400)]
    public string Description { get; set; } = null!;

    [InverseProperty("PermissionKeyNavigation")]
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
