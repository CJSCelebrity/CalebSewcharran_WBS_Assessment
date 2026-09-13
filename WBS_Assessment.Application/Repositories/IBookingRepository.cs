using WBS_Assessment.Core.Models;

namespace WBS_Assessment.Application.Repositories;

public interface IBookingRepository
{
    void Add(Booking booking);
    Booking? GetById(Guid id);
    IReadOnlyCollection<Booking> GetByUser(Guid userId);
    IReadOnlyCollection<Booking> GetAll();
}