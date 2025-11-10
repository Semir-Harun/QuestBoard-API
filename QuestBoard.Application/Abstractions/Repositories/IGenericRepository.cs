using QuestBoard.Domain.Entities;

namespace QuestBoard.Application.Abstractions.Repositories;

public interface IGenericRepository<TEntity>
    where TEntity : BaseEntity
{
    IQueryable<TEntity> Query();
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}
