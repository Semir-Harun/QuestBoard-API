using QuestBoard.Application.Abstractions.Repositories;
using QuestBoard.Domain.Entities;

namespace QuestBoard.Infrastructure.Persistence.Repositories;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(QuestDbContext context) : base(context)
    {
    }
}
