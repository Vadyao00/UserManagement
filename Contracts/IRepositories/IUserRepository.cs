using Domain.Entities;
using Domain.RequestFeatures;

namespace Contracts.IRepositories;

public interface IUserRepository
{
    Task<PagedList<User>> GetAllUsersAsync(UserParameters userParameters, bool trackChanges);
    Task<User?> GetUserByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllUsersWithoutMetaAsync(bool trackChanges);
    void CreateUser(User user);
    void DeleteUser(User user);
    void UpdateUser(User user);
    void Attach(User user);
}