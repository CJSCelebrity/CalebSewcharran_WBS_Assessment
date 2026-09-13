using System.Collections.Concurrent;
using WBS_Assessment.Application.Repositories;
using WBS_Assessment.Core.Interfaces;

namespace WBS_Assessment.Infrastructure.Repositories;

public class InMemoryRepository<T> : IRepository<T> where T : IBookingInformation
{
    private readonly ConcurrentDictionary<Guid, T> _items = new();

    public InMemoryRepository(IEnumerable<T> seedData)
    {
        foreach (var item in seedData)
        {
            _items[item.Id] = item;
        }
    }
    
    public T? GetById(Guid id) => _items.TryGetValue(id, out T item) ? item : default;

    public IReadOnlyCollection<T> GetAll() => _items.Values.ToList();
}