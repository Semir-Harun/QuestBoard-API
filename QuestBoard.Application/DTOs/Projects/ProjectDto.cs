using System;

namespace QuestBoard.Application.DTOs.Projects;

public sealed record ProjectDto(Guid Id, string Name, string? Description, DateTime CreatedAt, DateTime? UpdatedAt);
