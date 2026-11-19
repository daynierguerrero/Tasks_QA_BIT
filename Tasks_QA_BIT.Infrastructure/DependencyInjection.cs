using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tasks_QA_BIT.Application.Tasks;
using Tasks_QA_BIT.Infrastructure.Persistence;

namespace Tasks_QA_BIT.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("TasksDatabase")
            ?? throw new InvalidOperationException("Connection string 'TasksDatabase' is missing.");

        services.AddDbContext<TaskDbContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<ITaskRepository, TaskRepository>();
        return services;
    }
}
