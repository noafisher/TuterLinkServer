using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TuterLinkServer.Models;

[PrimaryKey("TeacherId", "StudentId")]
public partial class StudentToTeacher
{
    [Key]
    [Column("StudentID")]
    public int StudentId { get; set; }

    [Key]
    [Column("TeacherID")]
    public int TeacherId { get; set; }

    [Column("SubjectID")]
    public int? SubjectId { get; set; }

    [ForeignKey("StudentId")]
    [InverseProperty("StudentToTeachers")]
    public virtual Student Student { get; set; } = null!;

    [ForeignKey("SubjectId")]
    [InverseProperty("StudentToTeachers")]
    public virtual Subject? Subject { get; set; }

    [ForeignKey("TeacherId")]
    [InverseProperty("StudentToTeachers")]
    public virtual Teacher Teacher { get; set; } = null!;
}
