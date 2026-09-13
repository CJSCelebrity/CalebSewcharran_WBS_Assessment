namespace WBS_Assessment.Application.Dto;

public record CreateApartmentBookingRequest(Guid UserId, Guid ItemId, DateTime CheckIn, DateTime CheckOut);