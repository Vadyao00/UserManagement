using Contracts.IRepositories;
using Contracts.IServices;
using Domain.Entities;
using Domain.RequestFeatures;
using Domain.Responses;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IRepositoryManager _repositoryManager;

    public UserService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<ApiBaseResponse> GetAllUsersAsync(UserParameters userParameters, User currentUser)
    {
        if (!CheckUser(currentUser))
            return new BadUserBadRequestResponse();
        
        var users = await _repositoryManager.User.GetAllUsersAsync(userParameters, trackChanges: false);
        return new ApiOkResponse<List<User>>(users);
    }

    public async Task<ApiBaseResponse> GetUserByEmailAsync(string email, User currentUser)
    {
        if (!CheckUser(currentUser))
            return new BadUserBadRequestResponse();
        
        var existingUser = await _repositoryManager.User.GetUserByEmailAsync(email);

        if (existingUser == null)
        {
            return new InvalidEmailBadRequestResponse();
        }
        
        return new ApiOkResponse<User>(existingUser);
    }

    public async Task<ApiBaseResponse> DeleteUserAsync(string email, User currentUser)
    {
        if (!CheckUser(currentUser))
            return new BadUserBadRequestResponse();
        
        var user = await _repositoryManager.User.GetUserByEmailAsync(email);
        if (user != null)
        {
            _repositoryManager.User.DeleteUser(user);
            await _repositoryManager.SaveAsync();
        }
        else
        {
            return new InvalidEmailBadRequestResponse();
        }

        return new ApiOkResponse<User>(user);
    }

    public async Task<ApiBaseResponse> BlockUserAsync(string email, User currentUser)
    {
        if (!CheckUser(currentUser))
            return new BadUserBadRequestResponse();
        
        var user = await _repositoryManager.User.GetUserByEmailAsync(email);
        if (user != null)
        {
            user.Status = "blocked";
            _repositoryManager.User.UpdateUser(user);
            await _repositoryManager.SaveAsync();
        }
        else
        {
            return new InvalidEmailBadRequestResponse();
        }
        
        return new ApiOkResponse<User>(user);
    }

    public async Task<ApiBaseResponse> UnblockUserAsync(string email, User currentUser)
    {
        if (!CheckUser(currentUser))
            return new BadUserBadRequestResponse();
        
        var user = await _repositoryManager.User.GetUserByEmailAsync(email);
        if (user != null)
        {
            user.Status = "active";
            _repositoryManager.User.UpdateUser(user);
            await _repositoryManager.SaveAsync();
        }
        else
        {
            return new InvalidEmailBadRequestResponse();
        }
        
        return new ApiOkResponse<User>(user);
    }

    private bool CheckUser(User? user)
    {
        if (user == null)
        {
            return false;
        }

        if (user.Status == "blocked")
        {
            return false;
        }

        return true;
    }
}