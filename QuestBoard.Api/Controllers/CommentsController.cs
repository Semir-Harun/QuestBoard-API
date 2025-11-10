using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestBoard.Application.DTOs.Comments;
using QuestBoard.Application.Services;

namespace QuestBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly CommentService _commentService;

    public CommentsController(CommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("task/{taskId:guid}")]
    public async Task<ActionResult<IReadOnlyList<CommentDto>>> GetByTask(Guid taskId, CancellationToken cancellationToken)
    {
        var comments = await _commentService.ListByTaskAsync(taskId, cancellationToken);
        return Ok(comments);
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create([FromBody] CommentCreateDto dto, CancellationToken cancellationToken)
    {
        var authorId = GetUserId();
        var comment = await _commentService.CreateAsync(dto, authorId, cancellationToken);
        return CreatedAtAction(nameof(GetByTask), new { taskId = dto.TaskItemId }, comment);
    }

    private Guid GetUserId()
    {
        var idClaim = User.FindFirst("sub") ?? throw new InvalidOperationException("Missing subject claim.");
        return Guid.Parse(idClaim.Value);
    }
}
