using QuestBoard.Domain.Entities;

namespace QuestBoard.Application.Abstractions.Repositories;

public interface IProjectRepository : IGenericRepository<Project>
{
    Task<IReadOnlyList<Project>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
}
