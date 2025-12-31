using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

[PrimaryKey("IssueId", "CustomFieldId")]
public partial class IssueCustomFieldValue
{
    [Key]
    public Guid IssueId { get; set; }

    [Key]
    public Guid CustomFieldId { get; set; }

    public string? ValueText { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? ValueNumber { get; set; }

    public DateOnly? ValueDate { get; set; }

    public bool? ValueBool { get; set; }

    public Guid? ValueOptionId { get; set; }

    public string? ValueJson { get; set; }

    [Precision(3)]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("CustomFieldId")]
    [InverseProperty("IssueCustomFieldValues")]
    public virtual CustomField CustomField { get; set; } = null!;

    [ForeignKey("IssueId")]
    [InverseProperty("IssueCustomFieldValues")]
    public virtual Issue Issue { get; set; } = null!;

    [ForeignKey("ValueOptionId")]
    [InverseProperty("IssueCustomFieldValues")]
    public virtual CustomFieldOption? ValueOption { get; set; }
}
