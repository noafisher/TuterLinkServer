using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TutorLinkServer.Models;

public partial class Subject
{
    [Key]
    [Column("SubjectID")]
    public int SubjectId { get; set; }

    [StringLength(25)]
    public string SubjectName { get; set; } = null!;

    [InverseProperty("Subject")]
    public virtual ICollection<TeachersSubject> TeachersSubjects { get; set; } = new List<TeachersSubject>();
}
