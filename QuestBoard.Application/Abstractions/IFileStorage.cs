using QuestBoard.Application.DTOs.Common;

namespace QuestBoard.Application.Abstractions;

public interface IFileStorage
{
    Task<FileSaveResult> SaveAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
}
