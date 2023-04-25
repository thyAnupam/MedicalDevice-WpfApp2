using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WpfApp2.Repository.Models;

[Table("Patient")]
public partial class Patient
{
    [Key]
    [Column("PatientID")]
    public int PatientId { get; set; }

    [StringLength(50)]
    public string Firstname { get; set; } = null!;

    [StringLength(50)]
    public string Lastname { get; set; } = null!;

    [Column("DOB", TypeName = "date")]
    public DateTime Dob { get; set; }

    public int Gender { get; set; }

    public int Height { get; set; }

    [InverseProperty("Patient")]
    public virtual ICollection<Study> Studies { get; } = new List<Study>();
}
