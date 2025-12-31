using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

public partial class ProjectCounter
{
    [Key]
    public Guid ProjectId { get; set; }

    public int NextIssueNo { get; set; }

    public byte[] RowVer { get; set; } = null!;

    [ForeignKey("ProjectId")]
    [InverseProperty("ProjectCounter")]
    public virtual Project Project { get; set; } = null!;
}
