using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WpfApp2.Repository.Models;

[Keyless]
[Table("Video")]
public partial class Video
{
    [Column("SeriesID")]
    public int SeriesId { get; set; }

    [StringLength(50)]
    public string VideoName { get; set; } = null!;

    [StringLength(50)]
    public string VideoPath { get; set; } = null!;

    [ForeignKey("SeriesId")]
    public virtual Series Series { get; set; } = null!;
}
