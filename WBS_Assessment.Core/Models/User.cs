using WBS_Assessment.Core.Enum;

namespace WBS_Assessment.Core.Models;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public BookingType BookingType { get; set; }
}