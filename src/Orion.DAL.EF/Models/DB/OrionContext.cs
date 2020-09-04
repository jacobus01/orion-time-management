using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Orion.Common.Library.Encryption;

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

        public int CurrentUserId { get; set; }

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

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is ITrackable trackable)
                {
                    var now = DateTime.UtcNow;
                    var user = CurrentUserId;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.LastUpdatedAt = now;
                            trackable.LastUpdatedBy = user;
                            break;

                        case EntityState.Added:
                            trackable.CreatedAt = now;
                            trackable.CreatedBy = user;
                            trackable.LastUpdatedAt = now;
                            trackable.LastUpdatedBy = user;
                            break;

                        case EntityState.Deleted:
                            trackable.LastUpdatedAt = now;
                            trackable.LastUpdatedBy = user;
                            trackable.IsDeleted = true;
                            entry.State = EntityState.Modified;
                            break;
                    }
                }
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

            //Seed Data
            CurrentUserId = 1;

            modelBuilder.Entity<User>().HasData(
    new User
    {
        Id = 1,
        FirstName = "Root",
        LastName = "Admin",
        EmployeeNumber = "XXXXXXXXX",
        Email = "admin@admin.com",
        AppointmentDate = DateTime.Today,
        AccessGroupId = 1,
        ChangePasswordOnNextLogin = false,
        IsActive = true,
        CreatedAt = DateTime.Today,
        CreatedBy = 1,
        IsDeleted = false,
        RoleId = 1,
        UserName = "SuperUser",
        LastUpdatedAt = DateTime.Today,
        LastUpdatedBy = 1,
        LockoutEnabled = false,
        PasswordHash = Cipher.Encrypt("123456", Cipher.orionSalt)
    });

            modelBuilder.Entity<AccessGroup>().HasData(
                new AccessGroup
                {
                    Id = 1,
                    AccessGroupName = "Admin",
                    CreatedAt = DateTime.Today,
                    CreatedBy = 1,
                    LastUpdatedAt = DateTime.Today,
                    LastUpdatedBy = 1,
                    IsDeleted = false
                },
                new AccessGroup
                {
                    Id = 2,
                    AccessGroupName = "User",
                    CreatedAt = DateTime.Today,
                    CreatedBy = 1,
                    LastUpdatedAt = DateTime.Today,
                    LastUpdatedBy = 1,
                    IsDeleted = false
                }
                );

            modelBuilder.Entity<Role>( ).HasData(
                new Role
                {
                    Id = 1,
                    RoleName = "Casual Employee Level 1",
                    Rate = 0,
                    CreatedAt = DateTime.Today,
                    CreatedBy = 1,
                    LastUpdatedAt = DateTime.Today,
                    LastUpdatedBy = 1,
                    IsDeleted = false
                },
                new Role
                {
                    Id = 2,
                    RoleName = "Casual Employee Level 2",
                    Rate = 0,
                    CreatedAt = DateTime.Today,
                    CreatedBy = 1,
                    LastUpdatedAt = DateTime.Today,
                    LastUpdatedBy = 1,
                    IsDeleted = false
                },
                new Role
                {
                    Id = 3,
                    RoleName = "Manager",
                    Rate = 0,
                    CreatedAt = DateTime.Today,
                    CreatedBy = 1,
                    LastUpdatedAt = DateTime.Today,
                    LastUpdatedBy = 1,
                    IsDeleted = false
                }
                );


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
