using JobApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobApp.Infrastructure.Presistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Role).HasMaxLength(20);
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(j => j.Id);
            entity.Property(j => j.Title).IsRequired().HasMaxLength(100);
            entity.Property(j => j.Description).IsRequired();
            entity.Property(j => j.Company).IsRequired().HasMaxLength(100);
            entity.Property(j => j.Location).IsRequired().HasMaxLength(100);

            entity.HasOne(j => j.CreatedBy)
                  .WithMany()
                  .HasForeignKey(j => j.CreatedById)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<JobApplication>(entity =>
        {
            entity.HasKey(ja => ja.Id);
            entity.HasIndex(ja => new { ja.JobId, ja.ApplicantId }).IsUnique();

            entity.HasOne(ja => ja.Job)
                  .WithMany()
                  .HasForeignKey(ja => ja.JobId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ja => ja.Applicant)
                  .WithMany()
                  .HasForeignKey(ja => ja.ApplicantId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
