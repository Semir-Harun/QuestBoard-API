using System;

namespace QuestBoard.Application.DTOs.Tasks;

public sealed record TaskCreateDto(string Title, string? Description, QuestBoard.Domain.Enums.TaskStatus Status, Guid ProjectId);
