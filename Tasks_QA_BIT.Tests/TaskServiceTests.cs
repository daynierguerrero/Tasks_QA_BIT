using Tasks_QA_BIT.Application.Tasks;
using Tasks_QA_BIT.Domain;
using Xunit;

namespace Tasks_QA_BIT.Tests;

public sealed class TaskServiceTests
{
    [Fact]
    public async Task CreateTaskAsync_PersistsAndReturnsTask()
    {
        var repository = new FakeTaskRepository();
        var service = new TaskService(repository);

        var result = await service.CreateTaskAsync(Input("Preparar entrega"));

        Assert.Equal(1, result.Id);
        Assert.Equal("Preparar entrega", result.Title);
        Assert.Equal(TaskState.Pendiente, result.Status);
        Assert.Single(repository.Items);
    }

    [Fact]
    public async Task GetTasksAsync_AppliesFiltersAndPaginationMetadata()
    {
        var repository = new FakeTaskRepository();
        repository.Items.AddRange([
            MakeTask(1, "Tarea A", TaskState.Pendiente, "Ana"),
            MakeTask(2, "Tarea B", TaskState.EnCurso, "Ana"),
            MakeTask(3, "Tarea C", TaskState.Pendiente, "Luis")
        ]);
        var service = new TaskService(repository);

        var result = await service.GetTasksAsync(TaskState.Pendiente, "Ana", 1, 1);

        Assert.Single(result.Items);
        Assert.Equal("Tarea A", result.Items[0].Title);
        Assert.Equal(1, result.Page);
        Assert.Equal(1, result.PageSize);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal(1, result.TotalPages);
    }

    [Fact]
    public async Task UpdateAndDeleteTaskAsync_ChangesExistingTaskAndReportsMissingIds()
    {
        var repository = new FakeTaskRepository();
        repository.Items.Add(MakeTask(7, "Original", TaskState.Pendiente, "Ana"));
        var service = new TaskService(repository);

        var updated = await service.UpdateTaskAsync(7, Input("Actualizada", TaskState.Hecha));
        var deleted = await service.DeleteTaskAsync(7);
        var missing = await service.DeleteTaskAsync(7);

        Assert.NotNull(updated);
        Assert.Equal("Actualizada", updated.Title);
        Assert.Equal(TaskState.Hecha, updated.Status);
        Assert.True(deleted);
        Assert.False(missing);
        Assert.Empty(repository.Items);
    }

    private static TaskInput Input(string title, TaskState status = TaskState.Pendiente) => new()
    {
        Title = title,
        Description = "Descripción",
        Status = status,
        Responsible = "Ana",
        DueDate = new DateOnly(2026, 12, 31)
    };

    private static TaskItem MakeTask(int id, string title, TaskState status, string responsible) => new()
    {
        Id = id,
        Title = title,
        Description = "Descripción",
        Status = status,
        Responsible = responsible,
        DueDate = new DateOnly(2026, 12, 31)
    };

    private sealed class FakeTaskRepository : ITaskRepository
    {
        public List<TaskItem> Items { get; } = [];

        public Task<(IReadOnlyList<TaskItem> Items, int TotalCount)> GetPageAsync(
            TaskState? status, string? responsible, int page, int pageSize)
        {
            var filtered = Items.AsEnumerable();
            if (status.HasValue)
            {
                filtered = filtered.Where(task => task.Status == status.Value);
            }

            if (!string.IsNullOrWhiteSpace(responsible))
            {
                filtered = filtered.Where(task => task.Responsible == responsible);
            }

            var matching = filtered.OrderBy(task => task.Id).ToArray();
            var pageItems = matching.Skip((page - 1) * pageSize).Take(pageSize).ToArray();
            return Task.FromResult(((IReadOnlyList<TaskItem>)pageItems, matching.Length));
        }

        public Task<TaskItem?> GetByIdAsync(int id) =>
            Task.FromResult(Items.FirstOrDefault(task => task.Id == id));

        public Task<TaskItem> AddAsync(TaskItem task)
        {
            task.Id = Items.Count == 0 ? 1 : Items.Max(item => item.Id) + 1;
            Items.Add(task);
            return Task.FromResult(task);
        }

        public Task SaveChangesAsync() => Task.CompletedTask;

        public Task DeleteAsync(TaskItem task)
        {
            Items.Remove(task);
            return Task.CompletedTask;
        }
    }
}
