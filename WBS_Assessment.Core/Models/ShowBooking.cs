namespace WBS_Assessment.Core.Models;

public class ShowBooking : Booking
{
    public DateTime PerformanceTime { get; set; }
    
    public ShowBooking(Guid userId, Guid itemId, DateTime performanceTime)
        : base(userId, itemId)
    {
        Reschedule(performanceTime);
    }

    public void Reschedule(DateTime performanceTime)
    {
        EnsureBookingAmendable();
        PerformanceTime = performanceTime;
    }
}