using Microsoft.Extensions.DependencyInjection;
using Tasks_QA_BIT.Application.Tasks;

namespace Tasks_QA_BIT.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITaskService, TaskService>();
        return services;
    }
}
