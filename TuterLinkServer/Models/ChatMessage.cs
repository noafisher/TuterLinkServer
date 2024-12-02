using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TutorLinkServer.Models;

public partial class ChatMessage
{
    [Key]
    [Column("MessageID")]
    public int MessageId { get; set; }

    [Column("TeacherID")]
    public int TeacherId { get; set; }

    [Column("StudentID")]
    public int StudentId { get; set; }

    public bool IsTeacherSender { get; set; }

    [StringLength(500)]
    public string? MessageText { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TextTime { get; set; }

    [ForeignKey("StudentId")]
    [InverseProperty("ChatMessages")]
    public virtual Student Student { get; set; } = null!;

    [ForeignKey("TeacherId")]
    [InverseProperty("ChatMessages")]
    public virtual Teacher Teacher { get; set; } = null!;
}
