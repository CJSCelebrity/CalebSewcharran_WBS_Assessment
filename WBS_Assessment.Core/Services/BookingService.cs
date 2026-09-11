using WBS_Assessment.Core.Interfaces;

namespace WBS_Assessment.Core.Services;

public class BookingService : IBookingService
{
    public Task<T> AddBookingAsync<T>(T booking)
    {
        throw new NotImplementedException();
    }

    public Task DeleteBookingAsync(Guid bookingId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> GetAllBookingsAsync<T>()
    {
        throw new NotImplementedException();
    }

    public Task<T> UpdateBookingAsync<T>(T booking)
    {
        throw new NotImplementedException();
    }
}