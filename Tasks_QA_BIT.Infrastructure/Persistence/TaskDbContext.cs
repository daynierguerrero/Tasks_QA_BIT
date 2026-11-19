using Microsoft.EntityFrameworkCore;
using Tasks_QA_BIT.Domain;

namespace Tasks_QA_BIT.Infrastructure.Persistence;

public sealed class TaskDbContext(DbContextOptions<TaskDbContext> options) : DbContext(options)
{
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.ToTable("Tasks");
            entity.HasKey(task => task.Id);
            entity.Property(task => task.Title).HasMaxLength(200).IsRequired();
            entity.Property(task => task.Description).HasMaxLength(2000).IsRequired();
            entity.Property(task => task.Status).IsRequired();
            entity.Property(task => task.Responsible).HasMaxLength(100).IsRequired();
            entity.Property(task => task.DueDate).IsRequired();
        });
    }
}
