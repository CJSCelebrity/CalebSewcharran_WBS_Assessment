using WBS_Assessment.Core.Enum;

namespace WBS_Assessment.Core.Models;

public abstract class Booking
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ItemId { get; set; }
    public BookingStatus Status { get; set; }
}