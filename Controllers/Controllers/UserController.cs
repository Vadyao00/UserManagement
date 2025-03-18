using Contracts.IServices;
using Controllers.Extensions;
using Domain.Dtos;
using Domain.Entities;
using Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers;

[Route("api/users")]
[Authorize(Roles = "Administrator")]
[ApiController]
public class UserController : ApiControllerBase
{
    private readonly IServiceManager _serviceManager;

    public UserController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }
    
    private string GetTokenFromRequest()
    {
        var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
        
        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            return null;
        }
        return authorizationHeader.Substring("Bearer ".Length).Trim();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] UserParameters userParameters)
    {
        var token = GetTokenFromRequest();
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized();
        }

        var currentUser = _serviceManager.AuthenticationService.GetCurrentUserFromTokenAsync(token);
        
        var baseResult = await _serviceManager.UserService.GetAllUsersAsync(userParameters, currentUser.Result);
        if(!baseResult.Suссess)
            return ProccessError(baseResult);
        
        var result = baseResult.GetResult<List<User>>();
        
        var userDtos = new List<UserDto>();

        foreach (var user in result)
        {
            userDtos.Add(new UserDto
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                LastLogin = user.LastLogin,
                RegistrationTime = user.RegistrationTime,
                Status = user.Status
            });
        }

        return Ok(userDtos);
    }
    
    [HttpDelete("{email}")]
    public async Task<IActionResult> DeleteUser(string email)
    {
        var token = GetTokenFromRequest();
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized();
        }
        
        var currentUser = _serviceManager.AuthenticationService.GetCurrentUserFromTokenAsync(token);
        
        var baseResult = await _serviceManager.UserService.DeleteUserAsync(email, currentUser.Result);
        if(!baseResult.Suссess)
            return ProccessError(baseResult);
        
        return Ok();
    }
    
    [HttpPost("block/{email}")]
    public async Task<IActionResult> BlockUser(string email)
    {
        var token = GetTokenFromRequest();
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized();
        }
        
        var currentUser = _serviceManager.AuthenticationService.GetCurrentUserFromTokenAsync(token);
        
        var baseResult = await _serviceManager.UserService.BlockUserAsync(email, currentUser.Result);
        if(!baseResult.Suссess)
            return ProccessError(baseResult);
        
        return Ok();
    }
    
    [HttpPost("unblock/{email}")]
    public async Task<IActionResult> UnblockUser(string email)
    {
        var token = GetTokenFromRequest();
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized();
        }
        
        var currentUser = _serviceManager.AuthenticationService.GetCurrentUserFromTokenAsync(token);
        
        var baseResult = await _serviceManager.UserService.UnblockUserAsync(email, currentUser.Result);
        if(!baseResult.Suссess)
            return ProccessError(baseResult);
        
        return Ok();
    }
}