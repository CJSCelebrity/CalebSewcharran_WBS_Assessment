using System.Collections.Concurrent;
using WBS_Assessment.Application.Repositories;
using WBS_Assessment.Core.Models;

namespace WBS_Assessment.Infrastructure.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    private readonly ConcurrentDictionary<Guid, User> _users = new();

    public InMemoryUserRepository()
    {
        foreach (var user in SeedData.Users)
            _users[user.Id] = user;
    }

    public User? GetById(Guid id) => _users.TryGetValue(id, out var user) ? user : null;

    public IReadOnlyCollection<User> GetAll() => _users.Values.ToList();
}