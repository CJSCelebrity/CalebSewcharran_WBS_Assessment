using WBS_Assessment.Core.Models;

namespace WBS_Assessment.Application.Repositories;

public interface IUserRepository
{
    User? GetById(Guid id);
}