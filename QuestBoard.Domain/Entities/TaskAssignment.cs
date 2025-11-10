using System;

namespace QuestBoard.Domain.Entities;

public class TaskAssignment : BaseEntity
{
    public Guid TaskItemId { get; set; }
    public TaskItem Task { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
