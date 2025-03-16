using Contracts.IRepositories;
using Contracts.IServices;
using Domain.Entities;
using Domain.RequestFeatures;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IRepositoryManager _repositoryManager;

    public UserService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync(UserParameters userParameters)
    {
        return await _repositoryManager.User.GetAllUsersAsync(userParameters, trackChanges: false);
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _repositoryManager.User.GetUserByEmailAsync(email);
    }

    public async Task DeleteUserAsync(string email)
    {
        var user = await _repositoryManager.User.GetUserByEmailAsync(email);
        if (user != null)
        {
            _repositoryManager.User.DeleteUser(user);
            await _repositoryManager.SaveAsync();
        }
    }

    public async Task BlockUserAsync(string email)
    {
        var user = await _repositoryManager.User.GetUserByEmailAsync(email);
        if (user != null)
        {
            user.Status = "blocked";
            _repositoryManager.User.UpdateUser(user);
            await _repositoryManager.SaveAsync();
        }
    }

    public async Task UnblockUserAsync(string email)
    {
        var user = await _repositoryManager.User.GetUserByEmailAsync(email);
        if (user != null)
        {
            user.Status = "active";
            _repositoryManager.User.UpdateUser(user);
            await _repositoryManager.SaveAsync();
        }
    }
}