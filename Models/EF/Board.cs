using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

public partial class Board
{
    [Key]
    public Guid BoardId { get; set; }

    public Guid ProjectId { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(20)]
    public string BoardType { get; set; } = null!;

    public bool IsDefault { get; set; }

    [StringLength(2000)]
    public string? FilterJql { get; set; }

    [Precision(3)]
    public DateTime CreatedAt { get; set; }

    [Precision(3)]
    public DateTime UpdatedAt { get; set; }

    public byte[] RowVer { get; set; } = null!;

    [InverseProperty("Board")]
    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();

    [ForeignKey("ProjectId")]
    [InverseProperty("Boards")]
    public virtual Project Project { get; set; } = null!;

    [InverseProperty("Board")]
    public virtual ICollection<Sprint> Sprints { get; set; } = new List<Sprint>();
}
