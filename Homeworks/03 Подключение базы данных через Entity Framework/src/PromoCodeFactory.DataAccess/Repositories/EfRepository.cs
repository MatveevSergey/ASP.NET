using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.Core.Exceptions;
using System.Linq.Expressions;

namespace PromoCodeFactory.DataAccess.Repositories;

internal class EfRepository<T>(PromoCodeFactoryDbContext context) : IRepository<T> where T : BaseEntity
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    protected virtual IQueryable<T> ApplyIncludes(IQueryable<T> query) => query;

    private IQueryable<T> BuildQuery(bool withIncludes) =>
        withIncludes ? ApplyIncludes(_dbSet) : _dbSet;

    public async Task Add(T entity, CancellationToken ct)
    {
        _dbSet.Add(entity);
        await context.SaveChangesAsync(ct);
    }

    public async Task Delete(Guid id, CancellationToken ct)
    {
        var affected = await _dbSet
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(ct);

        if (affected == 0)
            throw new EntityNotFoundException(typeof(T), id);
    }

    public async Task<IReadOnlyCollection<T>> GetAll(bool withIncludes = false, CancellationToken ct = default)
    {
        return await BuildQuery(withIncludes)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<T?> GetById(Guid id, bool withIncludes = false, CancellationToken ct = default)
    {
        return await BuildQuery(withIncludes)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public async Task<IReadOnlyCollection<T>> GetByRangeId(IEnumerable<Guid> ids, bool withIncludes = false, CancellationToken ct = default)
    {
        var idList = ids.ToList();

        return await BuildQuery(withIncludes)
            .AsNoTracking()
            .Where(e => idList.Contains(e.Id))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<T>> GetWhere(Expression<Func<T, bool>> predicate, bool withIncludes = false, CancellationToken ct = default)
    {
        return await BuildQuery(withIncludes)
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync(ct);
    }

    public async Task Update(T entity, CancellationToken ct)
    {
        _dbSet.Update(entity);
        var affected = await context.SaveChangesAsync(ct);

        if (affected == 0)
            throw new EntityNotFoundException(typeof(T), entity.Id);
    }

}
