using Tasks_QA_BIT.Domain;

namespace Tasks_QA_BIT.Application.Tasks;

public interface ITaskService
{
    Task<PagedResult<TaskResponse>> GetTasksAsync(
        TaskState? status, string? responsible, int page, int pageSize);
    Task<TaskResponse?> GetTaskAsync(int id);
    Task<TaskResponse> CreateTaskAsync(TaskInput input);
    Task<TaskResponse?> UpdateTaskAsync(int id, TaskInput input);
    Task<bool> DeleteTaskAsync(int id);
}
