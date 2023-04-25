using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WpfApp2.Repository.Models;

public partial class Series
{
    [Key]
    [Column("SeriesID")]
    public int SeriesId { get; set; }

    [StringLength(50)]
    public string SeriesName { get; set; } = null!;

    [Column("StudyID")]
    public int StudyId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Created { get; set; }

    [ForeignKey("StudyId")]
    [InverseProperty("Series")]
    public virtual Study Study { get; set; } = null!;
}
