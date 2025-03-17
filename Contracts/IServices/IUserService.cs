using Domain.Entities;
using Domain.RequestFeatures;
using Domain.Responses;

namespace Contracts.IServices;

public interface IUserService
{
    Task<ApiBaseResponse> GetAllUsersAsync(UserParameters userParameters, User currentUser);
    
    Task<ApiBaseResponse> GetUserByEmailAsync(string email, User currentUser);
    
    Task<ApiBaseResponse> DeleteUserAsync(string email, User currentUser);
    
    Task<ApiBaseResponse> BlockUserAsync(string email, User currentUser);
    
    Task<ApiBaseResponse> UnblockUserAsync(string email, User currentUser);
}