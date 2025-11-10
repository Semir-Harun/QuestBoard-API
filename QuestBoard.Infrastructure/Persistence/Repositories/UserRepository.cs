using Microsoft.EntityFrameworkCore;
using QuestBoard.Application.Abstractions.Repositories;
using QuestBoard.Domain.Entities;

namespace QuestBoard.Infrastructure.Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(QuestDbContext context) : base(context)
    {
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => Context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
}
