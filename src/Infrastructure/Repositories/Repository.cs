using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Repositories;

internal abstract class Repository<TEntity>
    where TEntity : Entity
{
    protected readonly ApplicationDbContext DbContext;

    protected Repository(ApplicationDbContext dbContext) => DbContext = dbContext;

    protected virtual async Task<TEntity> PerformDatabaseOperationAsync<TEntity>(Func<Task<TEntity>> fnOp)
    {
        var result = await fnOp();
        return result;
    }

    public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<TEntity>()
            .FindAsync(new object[] { id }, cancellationToken);
    }

    public virtual void Add(TEntity entity)
    {
        DbContext.Add(entity);
    }
}