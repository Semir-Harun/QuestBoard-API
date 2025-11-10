using Microsoft.EntityFrameworkCore;
using QuestBoard.Application.Abstractions.Repositories;
using QuestBoard.Domain.Entities;

namespace QuestBoard.Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly QuestDbContext Context;

    protected GenericRepository(QuestDbContext context)
    {
        Context = context;
    }

    public virtual IQueryable<TEntity> Query() => Context.Set<TEntity>().AsQueryable();

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await Context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public virtual Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => Context.Set<TEntity>().AddAsync(entity, cancellationToken).AsTask();

    public virtual void Update(TEntity entity) => Context.Set<TEntity>().Update(entity);

    public virtual void Remove(TEntity entity) => Context.Set<TEntity>().Remove(entity);
}
