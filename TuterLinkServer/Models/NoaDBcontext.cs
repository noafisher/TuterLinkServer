﻿using System;
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
            entity.HasKey(e => e.MessageId).HasName("PK__ChatMess__C87C037CDFB66B28");

            entity.HasOne(d => d.Student).WithMany(p => p.ChatMessages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChatMessa__Stude__34C8D9D1");

            entity.HasOne(d => d.Teacher).WithMany(p => p.ChatMessages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChatMessa__Teach__33D4B598");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.LessonId).HasName("PK__Lessons__B084ACB09C08B9AE");

            entity.HasOne(d => d.Student).WithMany(p => p.Lessons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Lessons__Student__3C69FB99");

            entity.HasOne(d => d.Subject).WithMany(p => p.Lessons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Lessons__Subject__3D5E1FD2");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Lessons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Lessons__Teacher__3B75D760");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Reports__D5BD48E5984EA2B4");

            entity.HasOne(d => d.Student).WithMany(p => p.Reports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reports__Student__38996AB5");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Reports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reports__Teacher__37A5467C");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52A798C893850");

            entity.Property(e => e.IsAdmin).HasDefaultValue(false);
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK__Subjects__AC1BA38812D95491");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__Teachers__EDF25964FB1AB591");

            entity.Property(e => e.IsAdmin).HasDefaultValue(false);
        });

        modelBuilder.Entity<TeacherReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__TeacherR__74BC79AE9EF7F815");

            entity.HasOne(d => d.Student).WithMany(p => p.TeacherReviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeacherRe__Stude__30F848ED");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TeacherReviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeacherRe__Teach__300424B4");
        });

        modelBuilder.Entity<TeachersSubject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Teachers__3214EC279F026836");

            entity.HasOne(d => d.Subject).WithMany(p => p.TeachersSubjects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeachersS__Subje__2D27B809");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TeachersSubjects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeachersS__Teach__2C3393D0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
