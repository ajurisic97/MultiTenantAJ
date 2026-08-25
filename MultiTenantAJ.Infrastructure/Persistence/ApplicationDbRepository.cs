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

    public override Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Remove(entity);
        return Task.FromResult(0);
    }
}
