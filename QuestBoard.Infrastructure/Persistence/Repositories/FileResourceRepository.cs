using QuestBoard.Application.Abstractions.Repositories;
using QuestBoard.Domain.Entities;

namespace QuestBoard.Infrastructure.Persistence.Repositories;

public class FileResourceRepository : GenericRepository<FileResource>, IFileResourceRepository
{
    public FileResourceRepository(QuestDbContext context) : base(context)
    {
    }
}
