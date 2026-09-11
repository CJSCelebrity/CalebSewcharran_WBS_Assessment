using WBS_Assessment.Core.Enum;

namespace WBS_Assessment.Core.Models;

public class Show
{
    public Guid Id { get; set; }
    public string Venue { get; set; }
    public DateTime ShowStartDate { get; set; }
    public BookingType BookingType = BookingType.Show;
}