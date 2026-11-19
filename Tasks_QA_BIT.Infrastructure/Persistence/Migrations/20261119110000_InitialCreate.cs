using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tasks_QA_BIT.Infrastructure.Persistence.Migrations;

[DbContext(typeof(TaskDbContext))]
[Migration("20261119110000_InitialCreate")]
public sealed class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Tasks",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                Status = table.Column<int>(type: "INTEGER", nullable: false),
                Responsible = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                DueDate = table.Column<DateOnly>(type: "TEXT", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Tasks", task => task.Id));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Tasks");
    }
}
