using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

public partial class IssueType
{
    [Key]
    public int IssueTypeId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    public bool IsSubTask { get; set; }

    [InverseProperty("IssueType")]
    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();
}
