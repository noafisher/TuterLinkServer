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

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(25)]
    public string Pass { get; set; } = null!;

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    public bool? IsAdmin { get; set; }

    [StringLength(100)]
    public string UserAddress { get; set; } = null!;

    public int CurrentClass { get; set; }

    [InverseProperty("Student")]
    public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();

    [InverseProperty("Student")]
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    [InverseProperty("Student")]
    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    [InverseProperty("Student")]
    public virtual ICollection<TeacherReview> TeacherReviews { get; set; } = new List<TeacherReview>();
}
