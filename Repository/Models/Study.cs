using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WpfApp2.Repository.Models;

[Table("Study")]
public partial class Study
{
    [Key]
    [Column("StudyID")]
    public int StudyId { get; set; }

    [StringLength(50)]
    public string StudyName { get; set; } = null!;

    [Column("PatientID")]
    public int PatientId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Created { get; set; }

    [ForeignKey("PatientId")]
    [InverseProperty("Studies")]
    public virtual Patient Patient { get; set; } = null!;

    [InverseProperty("Study")]
    public virtual ICollection<Series> Series { get; } = new List<Series>();
}
