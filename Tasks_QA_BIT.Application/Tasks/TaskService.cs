using Tasks_QA_BIT.Domain;

namespace Tasks_QA_BIT.Application.Tasks;

public sealed class TaskService(ITaskRepository repository) : ITaskService
{
    public async Task<PagedResult<TaskResponse>> GetTasksAsync(
        TaskState? status, string? responsible, int page, int pageSize)
    {
        var (items, totalCount) = await repository.GetPageAsync(status, responsible, page, pageSize);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        return new PagedResult<TaskResponse>(
            items.Select(ToResponse).ToArray(), page, pageSize, totalCount, totalPages);
    }

    public async Task<TaskResponse?> GetTaskAsync(int id)
    {
        var task = await repository.GetByIdAsync(id);
        return task is null ? null : ToResponse(task);
    }

    public async Task<TaskResponse> CreateTaskAsync(TaskInput input)
    {
        var task = new TaskItem();
        Apply(task, input);
        return ToResponse(await repository.AddAsync(task));
    }

    public async Task<TaskResponse?> UpdateTaskAsync(int id, TaskInput input)
    {
        var task = await repository.GetByIdAsync(id);
        if (task is null)
        {
            return null;
        }

        Apply(task, input);
        await repository.SaveChangesAsync();
        return ToResponse(task);
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var task = await repository.GetByIdAsync(id);
        if (task is null)
        {
            return false;
        }

        await repository.DeleteAsync(task);
        return true;
    }

    private static void Apply(TaskItem task, TaskInput input)
    {
        task.Title = input.Title!;
        task.Description = input.Description!;
        task.Status = input.Status!.Value;
        task.Responsible = input.Responsible!;
        task.DueDate = input.DueDate!.Value;
    }

    private static TaskResponse ToResponse(TaskItem task) =>
        new(task.Id, task.Title, task.Description, task.Status, task.Responsible, task.DueDate);
}
