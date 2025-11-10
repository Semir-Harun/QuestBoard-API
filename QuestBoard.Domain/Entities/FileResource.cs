using System;

namespace QuestBoard.Domain.Entities;

public class FileResource : BaseEntity
{
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Length { get; set; }
    public string Path { get; set; } = string.Empty;
    public Guid? ProjectId { get; set; }
    public Project? Project { get; set; }
    public Guid? TaskItemId { get; set; }
    public TaskItem? Task { get; set; }
    public Guid UploadedById { get; set; }
    public User UploadedBy { get; set; } = null!;
}
