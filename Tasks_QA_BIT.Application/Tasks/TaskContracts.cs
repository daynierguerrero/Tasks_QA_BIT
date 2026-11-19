using System.ComponentModel.DataAnnotations;
using Tasks_QA_BIT.Domain;

namespace Tasks_QA_BIT.Application.Tasks;

public sealed class TaskInput
{
    [Required, StringLength(200)]
    public string? Title { get; init; }

    [Required, StringLength(2000)]
    public string? Description { get; init; }

    [Required, EnumDataType(typeof(TaskState))]
    public TaskState? Status { get; init; }

    [Required, StringLength(100)]
    public string? Responsible { get; init; }

    [Required]
    public DateOnly? DueDate { get; init; }
}

public sealed record TaskResponse(
    int Id,
    string Title,
    string Description,
    TaskState Status,
    string Responsible,
    DateOnly DueDate);

public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages);
