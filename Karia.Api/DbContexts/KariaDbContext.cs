using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Karia.Api.Entities;

#nullable disable

namespace Karia.Api.DbContexts
{
    public partial class KariaDbContext : DbContext
    {
        public KariaDbContext()
        {
        }

        public KariaDbContext(DbContextOptions<KariaDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Commenting> Commentings { get; set; }
        public virtual DbSet<Critic> Critics { get; set; }
        public virtual DbSet<Employer> Employers { get; set; }
        public virtual DbSet<Expert> Experts { get; set; }
        public virtual DbSet<Grouping> Groupings { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<WorkSample> WorkSamples { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ_NAME")
                    .IsUnique();

                entity.Property(e => e.Icon).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Commenting>(entity =>
            {
                entity.ToTable("Commenting");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.DateOfRegistration).HasColumnType("date");

                entity.Property(e => e.IsValid).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Employer)
                    .WithMany(p => p.Commentings)
                    .HasForeignKey(d => d.EmployerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Commenting_Employer");

                entity.HasOne(d => d.Expert)
                    .WithMany(p => p.Commentings)
                    .HasForeignKey(d => d.ExpertId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Commenting_Expert");
            });

            modelBuilder.Entity<Critic>(entity =>
            {
                entity.ToTable("Critic");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Text).HasMaxLength(500);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Employer)
                    .WithMany(p => p.Critics)
                    .HasForeignKey(d => d.EmployerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Critic_Employer");
            });

            modelBuilder.Entity<Employer>(entity =>
            {
                entity.ToTable("Employer");

                entity.HasIndex(e => e.PhoneNumber, "UQ_PhoneNumber_Emp")
                    .IsUnique();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ProfileImage).HasMaxLength(100);

                entity.Property(e => e.RegisterData).HasColumnType("date");
            });

            modelBuilder.Entity<Expert>(entity =>
            {
                entity.ToTable("Expert");

                entity.HasIndex(e => e.PhoneNumber, "UQ_PhoneNumber_Exp")
                    .IsUnique();

                entity.Property(e => e.Birthyear).HasColumnType("date");

                entity.Property(e => e.Description).HasMaxLength(400);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IsHasVehicle).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsMaster).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsValid).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Offers).HasDefaultValueSql("((0))");

                entity.Property(e => e.Orientation).HasMaxLength(120);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ProfileImage).HasMaxLength(100);

                entity.Property(e => e.Rate)
                    .HasColumnType("decimal(14, 12)")
                    .HasComputedColumnSql("([Scores]/[Count])", true);

                entity.Property(e => e.RegisterDate).HasColumnType("date");

                entity.Property(e => e.Scores).HasColumnType("decimal(3, 1)");
            });

            modelBuilder.Entity<Grouping>(entity =>
            {
                entity.ToTable("Grouping");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Groupings)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Grouping_Category");

                entity.HasOne(d => d.Expert)
                    .WithMany(p => p.Groupings)
                    .HasForeignKey(d => d.ExpertId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Grouping_Expert");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.Text).HasMaxLength(200);

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<Survey>(entity =>
            {
                entity.ToTable("Survey");

                entity.Property(e => e.Negative).HasDefaultValueSql("((0))");

                entity.Property(e => e.Positive).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Expert)
                    .WithMany(p => p.Surveys)
                    .HasForeignKey(d => d.ExpertId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Survey_Expert");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Surveys)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Survey_Question");
            });

            modelBuilder.Entity<WorkSample>(entity =>
            {
                entity.Property(e => e.SamplePhoto).HasMaxLength(100);

                entity.HasOne(d => d.Expert)
                    .WithMany(p => p.WorkSamples)
                    .HasForeignKey(d => d.ExpertId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_WorkSamples_Expert");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
