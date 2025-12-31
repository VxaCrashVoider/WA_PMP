using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

public partial class Priority
{
    [Key]
    public int PriorityId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    public int SortOrder { get; set; }

    [InverseProperty("Priority")]
    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();
}
