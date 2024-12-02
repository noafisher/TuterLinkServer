using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TutorLinkServer.Models;

public partial class Teacher
{
    [Key]
    public int TeacherId { get; set; }

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

    public double MaxDistance { get; set; }

    public bool? GoToStudent { get; set; }

    public bool? TeachAtHome { get; set; }

    public int Vetek { get; set; }

    public int PricePerHour { get; set; }

    [InverseProperty("Teacher")]
    public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();

    [InverseProperty("Teacher")]
    public virtual ICollection<TeacherReview> TeacherReviews { get; set; } = new List<TeacherReview>();

    [InverseProperty("Teacher")]
    public virtual ICollection<TeachersSubject> TeachersSubjects { get; set; } = new List<TeachersSubject>();
}
