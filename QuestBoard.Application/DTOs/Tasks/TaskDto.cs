using System;
using QuestBoard.Domain.Enums;

namespace QuestBoard.Application.DTOs.Tasks;

public sealed record TaskDto(Guid Id, string Title, string? Description, QuestBoard.Domain.Enums.TaskStatus Status, Guid ProjectId, DateTime CreatedAt, DateTime? UpdatedAt);
