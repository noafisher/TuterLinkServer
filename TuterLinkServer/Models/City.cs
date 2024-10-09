using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TutorLinkServer.Models;

[Table("City")]
public partial class City
{
    [Key]
    [Column("CityID")]
    public int CityId { get; set; }

    [StringLength(25)]
    public string? CityName { get; set; }
}
