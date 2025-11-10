namespace QuestBoard.Application.DTOs.Comments;

public sealed record CommentCreateDto(Guid TaskItemId, string Body);
