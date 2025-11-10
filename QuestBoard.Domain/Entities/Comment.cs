using System;

namespace QuestBoard.Domain.Entities;

public class Comment : BaseEntity
{
    public string Body { get; set; } = string.Empty;
    public Guid TaskItemId { get; set; }
    public TaskItem Task { get; set; } = null!;
    public Guid AuthorId { get; set; }
    public User Author { get; set; } = null!;
}
