using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WpfApp2.Repository.Models;

[Table("Image")]
public partial class Image
{
    [Key]
    [Column("ImagePrimaryKey")]
    public int ImagePrimaryKey { get; set; }

    [Column("SeriesID")]
    public int SeriesId { get; set; }

    [StringLength(50)]
    public string ImageName { get; set; } = null!;

    [StringLength(2147483645)]
    public string ImagePath { get; set; } = null!;

    [ForeignKey("SeriesId")]
    public virtual Series Series { get; set; } = null!;
}
