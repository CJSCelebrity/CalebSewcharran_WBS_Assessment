namespace WBS_Assessment.Core.Interfaces;

/*
 * TODO: In here, we need to check which booking the user wants to update
 */

public interface IBookingService
{
    Task<T> AddBookingAsync<T>(T booking);
    Task DeleteBookingAsync(Guid bookingId);
    Task<IEnumerable<T>> GetAllBookingsAsync<T>();
    Task<T> UpdateBookingAsync<T>(T booking);
}