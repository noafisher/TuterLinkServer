using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TuterLinkServer.Models;

public partial class Teacher
{
    [Key]
    [Column("TeacherID")]
    public int TeacherId { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? FirstName { get; set; }

    [StringLength(50)]
    public string? LastName { get; set; }

    [ForeignKey("Email")]
    [InverseProperty("Teachers")]
    public virtual User? EmailNavigation { get; set; }

    [InverseProperty("Teacher")]
    public virtual ICollection<StudentToTeacher> StudentToTeachers { get; set; } = new List<StudentToTeacher>();
}
