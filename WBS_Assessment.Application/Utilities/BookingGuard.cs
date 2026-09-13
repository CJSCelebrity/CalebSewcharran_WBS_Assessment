using WBS_Assessment.Application.Exceptions;
using WBS_Assessment.Application.Repositories;
using WBS_Assessment.Core.Interfaces;

namespace WBS_Assessment.Application.Utilities;

public class BookingGuard(IUserRepository userRepository)
{
    //Checks for both the user id and the item id, if they both exist
    public void EnsureExists<T>(Guid userId, Guid itemId, IRepository<T> items) where T : IBookingInformation
    {
        if (userRepository.GetById(userId) is null)
            throw new NotFoundException($"User {userId} is not found");
        if (items.GetById(itemId) is null)
            throw new NotFoundException($"{typeof(T).Name} - {itemId} is not found");
    }
}