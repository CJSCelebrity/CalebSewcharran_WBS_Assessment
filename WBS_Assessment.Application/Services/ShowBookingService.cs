using FluentValidation;
using WBS_Assessment.Application.Dto;
using WBS_Assessment.Application.Exceptions;
using WBS_Assessment.Application.Repositories;
using WBS_Assessment.Application.Utilities;
using WBS_Assessment.Core.Models;

namespace WBS_Assessment.Application.Services;

public class ShowBookingService(
    IBookingRepository bookingRepository,
    BookingGuard bookingGuard,
    IRepository<Show> showRepository,
    IValidator<CreateShowBookingRequest> createShowBookingRequestValidator)
{
    public Guid Create(CreateShowBookingRequest request)
    {
        createShowBookingRequestValidator.ValidateAndThrow(request);
        bookingGuard.EnsureExists(request.UserId, request.ItemId,  showRepository);

        var booking = new ShowBooking(request.UserId, request.ItemId, request.PerformanceTime);
        bookingRepository.Add(booking);
        return booking.Id;
    }

    public void Reschedule(Guid bookingId, DateTime performanceTime)
    {
        var booking = bookingRepository.GetById(bookingId) as ShowBooking
                      ?? throw new NotFoundException($"Show booking {bookingId} not found.");

        booking.Reschedule(performanceTime);
    }
}