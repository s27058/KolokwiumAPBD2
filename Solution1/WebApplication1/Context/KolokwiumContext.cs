using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Context;

public partial class KolokwiumContext : DbContext
{
    public KolokwiumContext()
    {
    }

    public KolokwiumContext(DbContextOptions<KolokwiumContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Models.Task> Tasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Default");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.IdProject).HasName("Project_pk");

            entity.ToTable("Project");

            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdDefaultAssigneeNavigation).WithMany(p => p.Projects)
                .HasForeignKey(d => d.IdDefaultAssignee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Project_User");
        });

        modelBuilder.Entity<Models.Task>(entity =>
        {
            entity.HasKey(e => e.IdTask).HasName("Task_pk");

            entity.ToTable("Task");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAssigneeNavigation).WithMany(p => p.TaskIdAssigneeNavigations)
                .HasForeignKey(d => d.IdAssignee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Task_User_Assignee");

            entity.HasOne(d => d.IdProjectNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.IdProject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Task_Project");

            entity.HasOne(d => d.IdReporterNavigation).WithMany(p => p.TaskIdReporterNavigations)
                .HasForeignKey(d => d.IdReporter)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Task_User_Reporter");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("User_pk");

            entity.ToTable("User");

            entity.Property(e => e.FirstName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasMany(d => d.IdProjects).WithMany(p => p.IdUsers)
                .UsingEntity<Dictionary<string, object>>(
                    "Access",
                    r => r.HasOne<Project>().WithMany()
                        .HasForeignKey("IdProject")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("ProjectAccess_Project"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("ProjectAccess_User"),
                    j =>
                    {
                        j.HasKey("IdUser", "IdProject").HasName("Access_pk");
                        j.ToTable("Access");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
