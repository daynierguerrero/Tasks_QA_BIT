using Tasks_QA_BIT.Domain;

namespace Tasks_QA_BIT.Application.Tasks;

public interface ITaskRepository
{
    Task<(IReadOnlyList<TaskItem> Items, int TotalCount)> GetPageAsync(
        TaskState? status, string? responsible, int page, int pageSize);
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem> AddAsync(TaskItem task);
    Task SaveChangesAsync();
    Task DeleteAsync(TaskItem task);
}
