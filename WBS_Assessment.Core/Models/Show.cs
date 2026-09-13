using WBS_Assessment.Core.Interfaces;

namespace WBS_Assessment.Core.Models;

public class Show : IBookingInformation
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Venue { get; init; } = string.Empty;
}