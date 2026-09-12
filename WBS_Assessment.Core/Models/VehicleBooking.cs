namespace WBS_Assessment.Core.Models;

public class VehicleBooking : Booking
{
    public DateTime Pickup { get; set; }
    public DateTime Dropoff { get; set; }
}