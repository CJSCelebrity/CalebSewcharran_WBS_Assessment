using WBS_Assessment.Core.Enums;

namespace WBS_Assessment.Core.Models;

public abstract class Booking
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid ItemId { get; init; }
    public BookingStatus Status { get; private set; } = BookingStatus.Reserved;
    public DateTime? CancelledAt { get; private set; }

    public void Cancel(DateTime? cancellationDate)
    {
        if(Status == BookingStatus.Cancelled)
            throw new InvalidOperationException("Booking is already cancelled");
        
        Status = BookingStatus.Cancelled;
        CancelledAt =  cancellationDate;
    }
    
    protected void EnsureBookingAmendable()
    {
        if(Status == BookingStatus.Cancelled)
            throw new InvalidOperationException("A cancelled booking cannot be amended");
    }
}