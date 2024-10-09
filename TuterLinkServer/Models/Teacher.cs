using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TutorLinkServer.Models;

public partial class Teacher
{
    [Key]
    [Column("TeacherID")]
    public int TeacherId { get; set; }

    public int? UserId { get; set; }

    [InverseProperty("Teacher")]
    public virtual ICollection<StudentToTeacher> StudentToTeachers { get; set; } = new List<StudentToTeacher>();

    [ForeignKey("UserId")]
    [InverseProperty("Teachers")]
    public virtual User? User { get; set; }
}
