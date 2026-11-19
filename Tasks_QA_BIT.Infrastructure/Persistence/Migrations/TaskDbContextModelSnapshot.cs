using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Tasks_QA_BIT.Domain;

namespace Tasks_QA_BIT.Infrastructure.Persistence.Migrations;

[DbContext(typeof(TaskDbContext))]
public sealed class TaskDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.11")
            .HasAnnotation("Relational:MaxIdentifierLength", 64);

        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.Property(task => task.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("INTEGER")
                .HasAnnotation("Sqlite:Autoincrement", true);
            entity.Property(task => task.Description)
                .IsRequired()
                .HasMaxLength(2000)
                .HasColumnType("TEXT");
            entity.Property(task => task.DueDate)
                .HasColumnType("TEXT");
            entity.Property(task => task.Responsible)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("TEXT");
            entity.Property(task => task.Status)
                .HasColumnType("INTEGER");
            entity.Property(task => task.Title)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("TEXT");
            entity.HasKey(task => task.Id);
            entity.ToTable("Tasks");
        });
    }
}
