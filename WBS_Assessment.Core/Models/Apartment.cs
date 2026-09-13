using WBS_Assessment.Core.Interfaces;

namespace WBS_Assessment.Core.Models;

public class Apartment : IBookingInformation
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Address { get;  init; } = string.Empty;
}