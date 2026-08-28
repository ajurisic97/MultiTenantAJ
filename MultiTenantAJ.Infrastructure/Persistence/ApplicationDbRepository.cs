using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Infrastructure.Persistence;

public class ApplicationDbRepository<T> : RepositoryBase<T>,IRepository<T>
    where T : class
{
    private readonly ApplicationDbContext _dbContext;
    public ApplicationDbRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<T>().AddAsync(entity, cancellationToken);

        return entity;
    }

    public override async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        var entityList = entities.ToList();

        await _dbContext.Set<T>().AddRangeAsync(entityList, cancellationToken);

        return entityList;
    }

    public override Task<int> UpdateAsync(T entity,CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Update(entity);

        return Task.FromResult(0);
    }

    public override Task<int> UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().UpdateRange(entities);

        return Task.FromResult(0);
    }

    public override Task<int> DeleteAsync( T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Remove(entity);

        return Task.FromResult(0);
    }

    public override Task<int> DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().RemoveRange(entities);

        return Task.FromResult(0);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException(
            "Use IUnitOfWork to save changes.");
    }

}
