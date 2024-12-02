using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TutorLinkServer.Models;

[Table("TeachersSubject")]
public partial class TeachersSubject
{
    [Column("TeacherID")]
    public int TeacherId { get; set; }

    [Column("SubjectID")]
    public int SubjectId { get; set; }

    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int MinClass { get; set; }

    public int MaxClass { get; set; }

    [ForeignKey("SubjectId")]
    [InverseProperty("TeachersSubjects")]
    public virtual Subject Subject { get; set; } = null!;

    [ForeignKey("TeacherId")]
    [InverseProperty("TeachersSubjects")]
    public virtual Teacher Teacher { get; set; } = null!;
}
