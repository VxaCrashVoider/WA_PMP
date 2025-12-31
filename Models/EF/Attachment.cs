using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WA_ProjectManagement.Models.EF;

public partial class Attachment
{
    [Key]
    public Guid AttachmentId { get; set; }

    public Guid IssueId { get; set; }

    public Guid UploadedById { get; set; }

    [StringLength(260)]
    public string FileName { get; set; } = null!;

    [StringLength(200)]
    public string? ContentType { get; set; }

    public long FileSizeBytes { get; set; }

    [StringLength(50)]
    public string StorageProvider { get; set; } = null!;

    [StringLength(1000)]
    public string StorageKey { get; set; } = null!;

    [Precision(3)]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("IssueId")]
    [InverseProperty("Attachments")]
    public virtual Issue Issue { get; set; } = null!;

    [ForeignKey("UploadedById")]
    [InverseProperty("Attachments")]
    public virtual User UploadedBy { get; set; } = null!;
}
