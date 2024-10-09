using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TutorLinkServer.Models;

public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(25)]
    public string? Pass { get; set; }

    [StringLength(50)]
    public string? FirstName { get; set; }

    [StringLength(50)]
    public string? LastName { get; set; }

    public bool? IsAdmin { get; set; }

    [Column("TypeID")]
    public int? TypeId { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    [InverseProperty("User")]
    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

    [ForeignKey("TypeId")]
    [InverseProperty("Users")]
    public virtual TypeUser? Type { get; set; }
}
