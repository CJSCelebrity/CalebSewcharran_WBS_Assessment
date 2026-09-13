using System.Collections.Concurrent;
using WBS_Assessment.Application.Repositories;
using WBS_Assessment.Core.Models;

namespace WBS_Assessment.Infrastructure.Repositories;

public class InMemoryBookingRepository : IBookingRepository
{
    private readonly ConcurrentDictionary<Guid, Booking> _bookings = new();

    public void Add(Booking booking)
    {
        if (!_bookings.TryAdd(booking.Id, booking))
            throw new InvalidOperationException($"Booking {booking.Id} already exists.");
    }

    public Booking? GetById(Guid id) =>
        _bookings.TryGetValue(id, out var booking) ? booking : null;

    public IReadOnlyCollection<Booking> GetByUser(Guid userId) =>
        _bookings.Values.Where(b => b.UserId == userId).ToList();

    public IReadOnlyCollection<Booking> GetAll() => _bookings.Values.ToList();
}