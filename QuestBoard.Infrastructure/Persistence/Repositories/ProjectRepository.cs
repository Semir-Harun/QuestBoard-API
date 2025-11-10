using Microsoft.EntityFrameworkCore;
using QuestBoard.Application.Abstractions.Repositories;
using QuestBoard.Domain.Entities;

namespace QuestBoard.Infrastructure.Persistence.Repositories;

public class ProjectRepository : GenericRepository<Project>, IProjectRepository
{
    public ProjectRepository(QuestDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Project>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
        => await Context.Projects.Where(p => p.OwnerId == ownerId).ToListAsync(cancellationToken);
}
