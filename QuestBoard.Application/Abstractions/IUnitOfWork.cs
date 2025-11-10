using QuestBoard.Application.Abstractions.Repositories;

namespace QuestBoard.Application.Abstractions;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IProjectRepository Projects { get; }
    ITaskRepository Tasks { get; }
    ICommentRepository Comments { get; }
    IFileResourceRepository Files { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
