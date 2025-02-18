using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TutorLinkServer.Models;

public partial class Lesson
{
    [Key]
    [Column("LessonID")]
    public int LessonId { get; set; }

    [Column("TeacherID")]
    public int TeacherId { get; set; }

    [Column("StudentID")]
    public int StudentId { get; set; }

    [Column("SubjectID")]
    public int SubjectId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TimeOfLesson { get; set; }

    [ForeignKey("StudentId")]
    [InverseProperty("Lessons")]
    public virtual Student Student { get; set; } = null!;

    [ForeignKey("SubjectId")]
    [InverseProperty("Lessons")]
    public virtual Subject Subject { get; set; } = null!;

    [ForeignKey("TeacherId")]
    [InverseProperty("Lessons")]
    public virtual Teacher Teacher { get; set; } = null!;
}
