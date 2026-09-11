using WBS_Assessment.Core.Enum;

namespace WBS_Assessment.Core.Models;

public class ApartmentBooking : Booking
{
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
}