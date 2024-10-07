using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TuterLinkServer.Models;

public partial class Student
{
    [Key]
    [Column("StudentID")]
    public int StudentId { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

  
    [ForeignKey("Email")]
    [InverseProperty("Students")]
    public virtual User? EmailNavigation { get; set; }

    [InverseProperty("Student")]
    public virtual ICollection<StudentToTeacher> StudentToTeachers { get; set; } = new List<StudentToTeacher>();
}
