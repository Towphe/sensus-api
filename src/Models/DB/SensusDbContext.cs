﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SensusAPI.Models.DB;

public partial class SensusDbContext : DbContext
{
    public SensusDbContext()
    {
    }

    public SensusDbContext(DbContextOptions<SensusDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Poll> Polls { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Answerid).HasName("answer_pkey");

            entity.ToTable("answer");

            entity.Property(e => e.Answerid)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("answerid");
            entity.Property(e => e.Ans).HasColumnName("ans");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_date");
            entity.Property(e => e.Pollid).HasColumnName("pollid");
            entity.Property(e => e.Questionid).HasColumnName("questionid");

            entity.HasOne(d => d.Poll).WithMany(p => p.Answers)
                .HasForeignKey(d => d.Pollid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("answer_pollid_fkey");

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.Questionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("answer_questionid_fkey");
        });

        modelBuilder.Entity<Poll>(entity =>
        {
            entity.HasKey(e => e.Pollid).HasName("poll_pkey");

            entity.ToTable("poll");

            entity.Property(e => e.Pollid)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("pollid");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_date");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Questionid).HasName("question_pkey");

            entity.ToTable("question");

            entity.Property(e => e.Questionid)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("questionid");
            entity.Property(e => e.Pollid).HasColumnName("pollid");
            entity.Property(e => e.Question1).HasColumnName("question");

            entity.HasOne(d => d.Poll).WithMany(p => p.Questions)
                .HasForeignKey(d => d.Pollid)
                .HasConstraintName("question_pollid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
