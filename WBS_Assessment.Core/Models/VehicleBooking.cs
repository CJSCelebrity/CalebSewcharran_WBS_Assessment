namespace WBS_Assessment.Core.Models;

public class VehicleBooking : Booking
{
    public DateTime Pickup { get; private set; }
    public DateTime Dropoff { get; private set; }

    public VehicleBooking(Guid userId, Guid itemId, DateTime pickUp, DateTime dropOff)
        : base(userId, itemId)
    {
        Reschedule(pickUp, dropOff);
    }

    public void Reschedule(DateTime pickUp, DateTime dropOff)
    {
        EnsureBookingAmendable();
        if(dropOff <=  pickUp)
            throw new ArgumentException("Drop off should not be before pick up");
        
        Pickup = pickUp;
        Dropoff = dropOff;
    }
}