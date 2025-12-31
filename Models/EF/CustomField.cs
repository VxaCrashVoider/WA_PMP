using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

public partial class CustomField
{
    [Key]
    public Guid CustomFieldId { get; set; }

    public Guid ProjectId { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(30)]
    public string FieldType { get; set; } = null!;

    public bool IsRequired { get; set; }

    [Precision(3)]
    public DateTime CreatedAt { get; set; }

    [InverseProperty("CustomField")]
    public virtual ICollection<CustomFieldOption> CustomFieldOptions { get; set; } = new List<CustomFieldOption>();

    [InverseProperty("CustomField")]
    public virtual ICollection<IssueCustomFieldValue> IssueCustomFieldValues { get; set; } = new List<IssueCustomFieldValue>();

    [ForeignKey("ProjectId")]
    [InverseProperty("CustomFields")]
    public virtual Project Project { get; set; } = null!;
}
