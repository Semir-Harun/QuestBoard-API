using System;

namespace QuestBoard.Application.DTOs.Comments;

public sealed record CommentDto(Guid Id, Guid TaskItemId, Guid AuthorId, string Body, DateTime CreatedAt);
