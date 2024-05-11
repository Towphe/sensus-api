using System;
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

    public virtual DbSet<Poll> Polls { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=sensus_db;user id=tope;password=pingu");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
