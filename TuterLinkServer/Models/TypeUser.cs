using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TuterLinkServer.Models;

[Table("TypeUser")]
public partial class TypeUser
{
    [Key]
    [Column("TypeID")]
    public int TypeId { get; set; }

    [StringLength(50)]
    public string? TypeName { get; set; }

    [InverseProperty("Type")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
