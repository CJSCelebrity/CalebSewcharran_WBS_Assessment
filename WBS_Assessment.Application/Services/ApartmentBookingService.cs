using FluentValidation;
using WBS_Assessment.Application.Dto;
using WBS_Assessment.Application.Exceptions;
using WBS_Assessment.Application.Repositories;
using WBS_Assessment.Application.Utilities;
using WBS_Assessment.Core.Models;

namespace WBS_Assessment.Application.Services;

public class ApartmentBookingService(
    IBookingRepository bookingRepository,
    BookingGuard bookingGuard,
    IRepository<Apartment> apartmentRepository,
    IValidator<CreateApartmentBookingRequest> createApartmentBookingRequestValidator)
{
    public Guid Create(CreateApartmentBookingRequest request)
    {
        createApartmentBookingRequestValidator.ValidateAndThrow(request);
        bookingGuard.EnsureExists(request.UserId, request.ItemId, apartmentRepository);

        if (apartmentRepository.GetById(request.ItemId) is null)
            throw new NotFoundException($"Apartment {request.ItemId} not found");

        var booking = new ApartmentBooking(request.UserId, request.ItemId, request.CheckIn, request.CheckOut);
        
        bookingRepository.Add(booking);
        return booking.Id;
    }

    public void Reschedule(Guid bookingId, DateTime checkIn, DateTime checkOut)
    {
        var booking = bookingRepository.GetById(bookingId) as ApartmentBooking ?? throw new NotFoundException($"Apartment booking {bookingId} not found");
        
        //The model will then recheck its own rules for rescheduling
        booking.Reschedule(checkIn, checkOut);
    }
}