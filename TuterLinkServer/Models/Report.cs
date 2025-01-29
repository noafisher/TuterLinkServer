using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TutorLinkServer.Models;

public partial class Report
{
    [Key]
    [Column("ReportID")]
    public int ReportId { get; set; }

    [Column("TeacherID")]
    public int TeacherId { get; set; }

    [Column("StudentID")]
    public int StudentId { get; set; }

    public bool ReportedByStudent { get; set; }

    [StringLength(500)]
    public string? ReportText { get; set; }

    [ForeignKey("StudentId")]
    [InverseProperty("Reports")]
    public virtual Student Student { get; set; } = null!;

    [ForeignKey("TeacherId")]
    [InverseProperty("Reports")]
    public virtual Teacher Teacher { get; set; } = null!;
}
