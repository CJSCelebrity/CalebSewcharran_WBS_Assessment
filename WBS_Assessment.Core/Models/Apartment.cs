using WBS_Assessment.Core.Enum;

namespace WBS_Assessment.Core.Models;

public class Apartment
{
    public string Name { get; set; }
    public DateTime ReservedStartDate { get; set; }
    public DateTime ReservedEndDate { get; set; }
    public BookingType BookingType = BookingType.Apartment;
}