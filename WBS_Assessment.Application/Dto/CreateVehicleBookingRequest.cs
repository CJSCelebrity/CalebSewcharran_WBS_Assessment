namespace WBS_Assessment.Application.Dto;

public record CreateVehicleBookingRequest(Guid UserId, Guid ItemId, DateTime Pickup, DateTime Dropoff);