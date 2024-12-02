using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TutorLinkServer.Models;

[Table("TeacherReview")]
public partial class TeacherReview
{
    [Key]
    [Column("ReviewID")]
    public int ReviewId { get; set; }

    [Column("TeacherID")]
    public int TeacherId { get; set; }

    [Column("StudentID")]
    public int StudentId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TimeOfReview { get; set; }

    [StringLength(400)]
    public string ReviewText { get; set; } = null!;

    public int Score { get; set; }

    [ForeignKey("StudentId")]
    [InverseProperty("TeacherReviews")]
    public virtual Student Student { get; set; } = null!;

    [ForeignKey("TeacherId")]
    [InverseProperty("TeacherReviews")]
    public virtual Teacher Teacher { get; set; } = null!;
}
