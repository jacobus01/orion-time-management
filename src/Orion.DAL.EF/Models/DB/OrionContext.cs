using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Orion.DAL.EF.Models.DB
{
    public partial class OrionContext : DbContext
    {
        public OrionContext()
        {
        }

        public OrionContext(DbContextOptions<OrionContext> options): base(options)
        {
        }

        public virtual DbSet<CapturedTime> CapturedTime { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<User> User { get; set; }

        public virtual DbSet<AccessGroup> AccessGroup { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost;Database=Orion;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CapturedTime>(entity =>
            {
                entity.Property(e => e.Rate).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.CapturedTime)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK_CapturedTime_Task");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CapturedTime)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_CapturedTime_User");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Rate).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.Property(e => e.Duration).HasColumnType("decimal(18, 1)");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email).HasColumnName("email");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ProfilePicture).HasColumnType("image");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_Role");

                entity.HasOne(d => d.AccessGroup)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.AccessGroupId)
                    .HasConstraintName("FK_User_AccessGroup");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
