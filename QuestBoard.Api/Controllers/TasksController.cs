using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestBoard.Application.DTOs.Common;
using QuestBoard.Application.DTOs.Tasks;
using QuestBoard.Application.Services;

namespace QuestBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly TaskService _taskService;

    public TasksController(TaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<TaskDto>>> Get([FromQuery] TaskQuery query, CancellationToken cancellationToken)
    {
        var result = await _taskService.ListAsync(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:guid}/assign")]
    [Authorize(Policy = QuestBoard.Application.Policies.AuthorizationPolicies.ManagerOrAdmin)]
    public async Task<IActionResult> Assign(Guid id, [FromBody] TaskAssignDto dto, CancellationToken cancellationToken)
    {
        await _taskService.AssignAsync(id, dto.UserId, cancellationToken);
        return NoContent();
    }
}
