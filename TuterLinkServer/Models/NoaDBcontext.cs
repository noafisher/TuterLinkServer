using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TutorLinkServer.Models;

public partial class NoaDBcontext : DbContext
{
    public NoaDBcontext()
    {
    }

    public NoaDBcontext(DbContextOptions<NoaDBcontext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentToTeacher> StudentToTeachers { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<TypeUser> TypeUsers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=TutorLink_DB;User ID=TaskAdminLogin;Password=NoaF1197;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__City__F2D21A9695277661");

            entity.Property(e => e.CityId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52A794048C673");

            entity.Property(e => e.StudentId).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithMany(p => p.Students).HasConstraintName("FK__Students__UserId__29572725");
        });

        modelBuilder.Entity<StudentToTeacher>(entity =>
        {
            entity.HasKey(e => new { e.TeacherId, e.StudentId, e.SubjectId }).HasName("PK__StudentT__E7721040135AA71C");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentToTeachers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentTo__Stude__30F848ED");

            entity.HasOne(d => d.Subject).WithMany(p => p.StudentToTeachers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentTo__Subje__32E0915F");

            entity.HasOne(d => d.Teacher).WithMany(p => p.StudentToTeachers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentTo__Teach__31EC6D26");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK__Subjects__AC1BA388B16FB30C");

            entity.Property(e => e.SubjectId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__Teachers__EDF259442242EA3D");

            entity.Property(e => e.TeacherId).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithMany(p => p.Teachers).HasConstraintName("FK__Teachers__UserId__2C3393D0");
        });

        modelBuilder.Entity<TypeUser>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__TypeUser__516F03951FA2A78F");

            entity.Property(e => e.TypeId).ValueGeneratedNever();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0704015C95");

            entity.HasOne(d => d.Type).WithMany(p => p.Users).HasConstraintName("FK__Users__TypeID__267ABA7A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
