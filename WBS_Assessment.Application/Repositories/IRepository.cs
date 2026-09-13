using WBS_Assessment.Core.Interfaces;

namespace WBS_Assessment.Application.Repositories;

public interface IRepository<T> where T : IBookingInformation
{
    T? GetById(Guid id);
    IReadOnlyCollection<T> GetAll();
}