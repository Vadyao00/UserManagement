using Domain.Entities;
using Domain.RequestFeatures;

namespace Contracts.IServices;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync(UserParameters userParameters);
    
    Task<User> GetUserByEmailAsync(string email);
    
    Task DeleteUserAsync(string email);
    
    Task BlockUserAsync(string email);
    
    Task UnblockUserAsync(string email);
}