using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WpfApp2.Repository.Models;

namespace WpfApp2.Repository;

public partial class MoDbContext : DbContext
{
    public MoDbContext()
    {
    }

    public MoDbContext(DbContextOptions<MoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Series> Series { get; set; }

    public virtual DbSet<Study> Studies { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\\\\\\\MSSQLLocalDB;Server=IN05N0018H; Initial Catalog=MedicalDevice;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasOne(d => d.Series).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Image_Series");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasOne(d => d.Series).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Report_Series");
        });

        modelBuilder.Entity<Series>(entity =>
        {
            entity.HasOne(d => d.Study).WithMany(p => p.Series)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Series_Study");
        });

        modelBuilder.Entity<Study>(entity =>
        {
            entity.HasOne(d => d.Patient).WithMany(p => p.Studies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Study_Patient");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_User");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasOne(d => d.Series).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Video_Series");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
