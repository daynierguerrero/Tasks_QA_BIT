using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Tasks_QA_BIT.Application.Tasks;
using Tasks_QA_BIT.Domain;

namespace Tasks_QA_BIT.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TasksController(ITaskService taskService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<TaskResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<TaskResponse>>> GetTasks(
        [FromQuery, EnumDataType(typeof(TaskState))] TaskState? status,
        [FromQuery, StringLength(100)] string? responsible,
        [FromQuery, Range(1, int.MaxValue)] int page = 1,
        [FromQuery, Range(1, 100)] int pageSize = 10)
    {
        var result = await taskService.GetTasksAsync(status, responsible, page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponse>> GetTask(int id)
    {
        var task = await taskService.GetTaskAsync(id);
        return task is null ? NotFound() : Ok(task);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskResponse>> CreateTask(TaskInput input)
    {
        var task = await taskService.CreateTaskAsync(input);
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponse>> UpdateTask(int id, TaskInput input)
    {
        var task = await taskService.UpdateTaskAsync(id, input);
        return task is null ? NotFound() : Ok(task);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var deleted = await taskService.DeleteTaskAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
