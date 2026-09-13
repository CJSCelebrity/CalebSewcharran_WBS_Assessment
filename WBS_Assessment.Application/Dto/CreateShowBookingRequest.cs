namespace WBS_Assessment.Application.Dto;

public record CreateShowBookingRequest(Guid UserId, Guid ItemId, DateTime PerformanceTime);