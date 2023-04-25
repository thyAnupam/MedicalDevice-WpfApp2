using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WpfApp2.Repository.Models;

[Table("User")]
public partial class User
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [StringLength(50)]
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    [Column("RoleID")]
    public int RoleId { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    
    

    [StringLength(50)]
    public string? Email { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;
}
