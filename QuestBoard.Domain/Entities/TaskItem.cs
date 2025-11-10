using System;
using System.Collections.Generic;

namespace QuestBoard.Domain.Entities;

public class TaskItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public QuestBoard.Domain.Enums.TaskStatus Status { get; set; } = QuestBoard.Domain.Enums.TaskStatus.ToDo;
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<TaskAssignment> Assignees { get; set; } = new List<TaskAssignment>();
    public ICollection<FileResource> Attachments { get; set; } = new List<FileResource>();
}
