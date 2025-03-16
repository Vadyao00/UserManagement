using Domain.Entities;
using Domain.RequestFeatures;

namespace Contracts.IRepositories;

public interface IUserRepository
{
    Task<PagedList<User>> GetAllUsersAsync(UserParameters userParameters, bool trackChanges);
    Task<IEnumerable<User>> GetAllUsersWithoutMetaAsync(bool trackChanges);
    void CreateUser(User user);
    void DeleteUser(User user);
    void Attach(User user);
}