using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TutorLinkServer.Models;

public partial class Student
{
    [Key]
    [Column("StudentID")]
    public int StudentId { get; set; }

    public int? UserId { get; set; }

    [InverseProperty("Student")]
    public virtual ICollection<StudentToTeacher> StudentToTeachers { get; set; } = new List<StudentToTeacher>();

    [ForeignKey("UserId")]
    [InverseProperty("Students")]
    public virtual User? User { get; set; }
}
