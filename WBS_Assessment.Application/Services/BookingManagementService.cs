using WBS_Assessment.Application.Exceptions;
using WBS_Assessment.Application.Repositories;
using WBS_Assessment.Core.Models;

namespace WBS_Assessment.Application.Services;

public class BookingManagementService(IBookingRepository bookingRepository, IUserRepository usersRepository)
{
    public void Cancel(Guid bookingId)
    {
        var booking = bookingRepository.GetById(bookingId)
                      ?? throw new NotFoundException($"Booking {bookingId} not found.");
        
        booking.Cancel(DateTime.UtcNow);
    }
 
    public Booking GetById(Guid bookingId)
    {
        return bookingRepository.GetById(bookingId)
               ?? throw new NotFoundException($"Booking {bookingId} not found.");
    }
 
    public IReadOnlyCollection<Booking> GetForUser(Guid userId)
    {
        if (usersRepository.GetById(userId) is null)
            throw new NotFoundException($"User {userId} not found.");
 
        return bookingRepository.GetByUser(userId);
    }
 
    public IReadOnlyCollection<Booking> GetAll() => bookingRepository.GetAll();
}

