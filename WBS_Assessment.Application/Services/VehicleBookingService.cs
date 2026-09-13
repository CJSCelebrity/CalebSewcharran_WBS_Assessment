using FluentValidation;
using WBS_Assessment.Application.Dto;
using WBS_Assessment.Application.Exceptions;
using WBS_Assessment.Application.Repositories;
using WBS_Assessment.Application.Utilities;
using WBS_Assessment.Core.Models;

namespace WBS_Assessment.Application.Services;

public class VehicleBookingService(
    IBookingRepository bookingRepository,
    IRepository<Vehicle> vehicleRepository,
    BookingGuard bookingGuard,
    IValidator<CreateVehicleBookingRequest> createVehicleBookingRequestValidator)
{
    public Guid Create(CreateVehicleBookingRequest request)
    {
        createVehicleBookingRequestValidator.ValidateAndThrow(request);
        bookingGuard.EnsureExists(request.UserId, request.ItemId, vehicleRepository);

        var booking = new VehicleBooking(request.UserId, request.ItemId, request.Pickup, request.Dropoff);
        
        bookingRepository.Add(booking);
        return booking.Id;
    }

    public void Reschedule(Guid bookingId, DateTime pickUp, DateTime dropOff)
    {
        var booking = bookingRepository.GetById(bookingId) as VehicleBooking 
                      ?? throw new NotFoundException($"Vehicle booking {bookingId} not found");
        
        booking.Reschedule(pickUp, dropOff);
    }
}