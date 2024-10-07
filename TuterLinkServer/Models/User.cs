using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TuterLinkServer.Models;

public partial class User
{
    [Key]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(25)]
    public string? Pass { get; set; }

    [Column("TypeID")]
    public int? TypeId { get; set; }
    [StringLength(50)]
    public string? FirstName { get; set; }

    [StringLength(50)]
    public string? LastName { get; set; }


    [InverseProperty("EmailNavigation")]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    [InverseProperty("EmailNavigation")]
    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

    [ForeignKey("TypeId")]
    [InverseProperty("Users")]
    public virtual TypeUser? Type { get; set; }
}
