using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestBoard.Application.DTOs.Projects;
using QuestBoard.Application.Services;

namespace QuestBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly ProjectService _projectService;

    public ProjectsController(ProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProjectDto>>> Get(CancellationToken cancellationToken)
    {
        var ownerId = GetUserId();
        var projects = await _projectService.ListAsync(ownerId, cancellationToken);
        return Ok(projects);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProjectDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var project = await _projectService.GetByIdAsync(id, cancellationToken);
        return project is null ? NotFound() : Ok(project);
    }

    [HttpPost]
    [Authorize(Policy = QuestBoard.Application.Policies.AuthorizationPolicies.ManagerOrAdmin)]
    public async Task<ActionResult<ProjectDto>> Create([FromBody] ProjectCreateDto dto, CancellationToken cancellationToken)
    {
        var ownerId = GetUserId();
        var project = await _projectService.CreateAsync(dto, ownerId, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = QuestBoard.Application.Policies.AuthorizationPolicies.ManagerOrAdmin)]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProjectUpdateDto dto, CancellationToken cancellationToken)
    {
        await _projectService.UpdateAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = QuestBoard.Application.Policies.AuthorizationPolicies.AdminOnly)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _projectService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    private Guid GetUserId()
    {
        var idClaim = User.FindFirst("sub") ?? throw new InvalidOperationException("Missing subject claim.");
        return Guid.Parse(idClaim.Value);
    }
}
