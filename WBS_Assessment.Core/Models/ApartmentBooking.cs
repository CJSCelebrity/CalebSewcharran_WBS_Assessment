namespace WBS_Assessment.Core.Models;

public class ApartmentBooking : Booking
{
    public DateTime CheckIn { get; private set; }
    public DateTime CheckOut { get; private set; }

    public ApartmentBooking(Guid userId, Guid itemId, DateTime checkIn, DateTime checkOut) : base(userId, itemId)
    {
        Reschedule(checkIn, checkOut);
    }

    public void Reschedule(DateTime checkIn, DateTime checkOut)
    {
        EnsureBookingAmendable();
        if(checkOut <=  checkIn)
            throw new ArgumentException("Check-out must be after check-in");
        
        CheckIn = checkIn;
        CheckOut = checkOut;
    }
}