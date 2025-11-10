using Microsoft.EntityFrameworkCore;
using QuestBoard.Application.Abstractions.Repositories;
using QuestBoard.Domain.Entities;

namespace QuestBoard.Infrastructure.Persistence.Repositories;

public class TaskRepository : GenericRepository<TaskItem>, ITaskRepository
{
    public TaskRepository(QuestDbContext context) : base(context)
    {
    }

    public async Task AssignUserAsync(Guid taskId, Guid userId, CancellationToken cancellationToken = default)
    {
        var exists = await Context.TaskAssignments
            .AnyAsync(a => a.TaskItemId == taskId && a.UserId == userId, cancellationToken);

        if (exists)
        {
            return;
        }

        var assignment = new TaskAssignment
        {
            TaskItemId = taskId,
            UserId = userId,
        };

        await Context.TaskAssignments.AddAsync(assignment, cancellationToken);
    }
}
