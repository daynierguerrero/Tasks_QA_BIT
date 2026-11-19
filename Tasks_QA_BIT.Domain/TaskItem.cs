namespace Tasks_QA_BIT.Domain;

public sealed class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskState Status { get; set; }
    public string Responsible { get; set; } = string.Empty;
    public DateOnly DueDate { get; set; }
}
