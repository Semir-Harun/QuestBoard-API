using QuestBoard.Application.Abstractions;
using QuestBoard.Application.Abstractions.Repositories;
using QuestBoard.Infrastructure.Persistence.Repositories;

namespace QuestBoard.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly QuestDbContext _context;

    public UnitOfWork(
        QuestDbContext context,
        IUserRepository users,
        IProjectRepository projects,
        ITaskRepository tasks,
        ICommentRepository comments,
        IFileResourceRepository files)
    {
        _context = context;
        Users = users;
        Projects = projects;
        Tasks = tasks;
        Comments = comments;
        Files = files;
    }

    public IUserRepository Users { get; }
    public IProjectRepository Projects { get; }
    public ITaskRepository Tasks { get; }
    public ICommentRepository Comments { get; }
    public IFileResourceRepository Files { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}
