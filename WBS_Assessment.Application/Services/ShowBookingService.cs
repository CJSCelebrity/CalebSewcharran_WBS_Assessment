using WBS_Assessment.Core.Models;

namespace WBS_Assessment.Application.Services;

public class ShowBookingService(Guid userId, Guid itemId) : Booking(userId, itemId)
{
    
}