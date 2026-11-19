using Microsoft.EntityFrameworkCore;
using Tasks_QA_BIT.Application.Tasks;
using Tasks_QA_BIT.Domain;

namespace Tasks_QA_BIT.Infrastructure.Persistence;

public sealed class TaskRepository(TaskDbContext dbContext) : ITaskRepository
{
    public async Task<(IReadOnlyList<TaskItem> Items, int TotalCount)> GetPageAsync(
        TaskState? status, string? responsible, int page, int pageSize)
    {
        IQueryable<TaskItem> query = dbContext.Tasks.AsNoTracking();
        if (status.HasValue)
        {
            query = query.Where(task => task.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(responsible))
        {
            query = query.Where(task => task.Responsible == responsible);
        }

        var totalCount = await query.CountAsync();
        var offset = (int)Math.Min((long)(page - 1) * pageSize, int.MaxValue);
        var items = await query
            .OrderBy(task => task.Id)
            .Skip(offset)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }

    public Task<TaskItem?> GetByIdAsync(int id) =>
        dbContext.Tasks.FirstOrDefaultAsync(task => task.Id == id);

    public async Task<TaskItem> AddAsync(TaskItem task)
    {
        dbContext.Tasks.Add(task);
        await dbContext.SaveChangesAsync();
        return task;
    }

    public Task SaveChangesAsync() => dbContext.SaveChangesAsync();

    public async Task DeleteAsync(TaskItem task)
    {
        dbContext.Tasks.Remove(task);
        await dbContext.SaveChangesAsync();
    }
}
