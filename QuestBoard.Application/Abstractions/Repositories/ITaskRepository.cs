using QuestBoard.Domain.Entities;

namespace QuestBoard.Application.Abstractions.Repositories;

public interface ITaskRepository : IGenericRepository<TaskItem>
{
    Task AssignUserAsync(Guid taskId, Guid userId, CancellationToken cancellationToken = default);
}
