using Domain.Entities;
using Domain.RequestFeatures;
using Domain.Responses;

namespace Contracts.IServices;

public interface IUserService
{
    Task<ApiBaseResponse> GetAllUsersAsync(UserParameters userParameters);
    
    Task<ApiBaseResponse> GetUserByEmailAsync(string email);
    
    Task<ApiBaseResponse> DeleteUserAsync(string email);
    
    Task<ApiBaseResponse> BlockUserAsync(string email);
    
    Task<ApiBaseResponse> UnblockUserAsync(string email);
}