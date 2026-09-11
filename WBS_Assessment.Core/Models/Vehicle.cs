using WBS_Assessment.Core.Enum;

namespace WBS_Assessment.Core.Models;

public class Vehicle
{
    public Guid Id { get; set; }
    public string VehicleType { get; set; }
    public DateTime PickupDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public BookingType BookingType = BookingType.Vehicle;
}