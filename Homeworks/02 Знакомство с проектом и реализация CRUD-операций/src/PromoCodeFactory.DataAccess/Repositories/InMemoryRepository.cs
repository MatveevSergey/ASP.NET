using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.Core.Exceptions;
using System.Collections.Concurrent;

namespace PromoCodeFactory.DataAccess.Repositories;

public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly ConcurrentDictionary<Guid, T> _data;

    public InMemoryRepository(IEnumerable<T> data)
    {
        _data = new ConcurrentDictionary<Guid, T>(data.Select(e => new KeyValuePair<Guid, T>(e.Id, e)));
    }
    public Task<IReadOnlyCollection<T>> GetAll(CancellationToken ct)
    {
        return Task.FromResult((IReadOnlyCollection<T>)_data.Values);
    }

    public Task<T?> GetById(Guid id, CancellationToken ct)
    {
        _data.TryGetValue(id, out var entity);

        return Task.FromResult(entity);
    }

    public Task Add(T entity, CancellationToken ct)
    {
        _data[entity.Id] = entity;

        return Task.CompletedTask;
    }

    public Task Update(T entity, CancellationToken ct)
    {
        if (!_data.TryGetValue(entity.Id, out _))
        {
            throw new EntityNotFoundException<T>(entity.Id);
        }

        _data[entity.Id] = entity;

        // соблюдается атомарность, но избыточно для этого проекта
        //if (!_data.TryGetValue(entity.Id, out var existingEntity))
        //{
        //    throw new EntityNotFoundException<T>(entity.Id);
        //}

        //if (!_data.TryUpdate(entity.Id, entity, existingEntity))
        //{
        //    throw new EntityNotFoundException<T>(entity.Id);
        //}

        return Task.CompletedTask;
    }

    public Task Delete(Guid id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
