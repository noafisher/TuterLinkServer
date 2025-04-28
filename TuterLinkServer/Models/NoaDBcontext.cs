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

    public virtual DbSet<ChatMessage> ChatMessages { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<TeacherReview> TeacherReviews { get; set; }

    public virtual DbSet<TeachersSubject> TeachersSubjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=TutorLink_DB;User ID=TaskAdminLogin;Password=NoaF1197;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__ChatMess__C87C037C3F9BABF2");

            entity.HasOne(d => d.Student).WithMany(p => p.ChatMessages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChatMessa__Stude__36B12243");

            entity.HasOne(d => d.Teacher).WithMany(p => p.ChatMessages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChatMessa__Teach__35BCFE0A");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.LessonId).HasName("PK__Lessons__B084ACB0BB2EA4B0");

            entity.HasOne(d => d.Student).WithMany(p => p.Lessons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Lessons__Student__3F466844");

            entity.HasOne(d => d.Subject).WithMany(p => p.Lessons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Lessons__Subject__403A8C7D");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Lessons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Lessons__Teacher__3E52440B");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Reports__D5BD48E54579301A");

            entity.HasOne(d => d.Student).WithMany(p => p.Reports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reports__Student__3A81B327");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Reports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reports__Teacher__398D8EEE");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52A7987AB5B95");

            entity.Property(e => e.IsAdmin).HasDefaultValue(false);
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK__Subjects__AC1BA38876BF87BE");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__Teachers__EDF259640B2B7BE2");

            entity.Property(e => e.IsAdmin).HasDefaultValue(false);
        });

        modelBuilder.Entity<TeacherReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__TeacherR__74BC79AE26C163F8");

            entity.HasOne(d => d.Student).WithMany(p => p.TeacherReviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeacherRe__Stude__32E0915F");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TeacherReviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeacherRe__Teach__31EC6D26");
        });

        modelBuilder.Entity<TeachersSubject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Teachers__3214EC27ADDB6310");

            entity.HasOne(d => d.Subject).WithMany(p => p.TeachersSubjects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeachersS__Subje__2F10007B");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TeachersSubjects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeachersS__Teach__2E1BDC42");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
