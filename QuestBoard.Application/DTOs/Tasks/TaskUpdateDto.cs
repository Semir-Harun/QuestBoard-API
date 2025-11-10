namespace QuestBoard.Application.DTOs.Tasks;

public sealed record TaskUpdateDto(string Title, string? Description, QuestBoard.Domain.Enums.TaskStatus Status);
