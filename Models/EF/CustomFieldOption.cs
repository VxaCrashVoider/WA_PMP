using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

public partial class CustomFieldOption
{
    [Key]
    public Guid OptionId { get; set; }

    public Guid CustomFieldId { get; set; }

    [StringLength(200)]
    public string Value { get; set; } = null!;

    public int SortOrder { get; set; }

    [ForeignKey("CustomFieldId")]
    [InverseProperty("CustomFieldOptions")]
    public virtual CustomField CustomField { get; set; } = null!;

    [InverseProperty("ValueOption")]
    public virtual ICollection<IssueCustomFieldValue> IssueCustomFieldValues { get; set; } = new List<IssueCustomFieldValue>();
}
