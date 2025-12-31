using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

public partial class IssueComment
{
    [Key]
    public Guid CommentId { get; set; }

    public Guid IssueId { get; set; }

    public Guid AuthorId { get; set; }

    public string Body { get; set; } = null!;

    public bool IsEdited { get; set; }

    [Precision(3)]
    public DateTime CreatedAt { get; set; }

    [Precision(3)]
    public DateTime UpdatedAt { get; set; }

    public byte[] RowVer { get; set; } = null!;

    [ForeignKey("AuthorId")]
    [InverseProperty("IssueComments")]
    public virtual User Author { get; set; } = null!;

    [ForeignKey("IssueId")]
    [InverseProperty("IssueComments")]
    public virtual Issue Issue { get; set; } = null!;
}
