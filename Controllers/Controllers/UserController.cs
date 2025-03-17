using Contracts.IServices;
using Controllers.Extensions;
using Domain.Dtos;
using Domain.Entities;
using Domain.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ApiControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] UserParameters userParameters)
    {
        var baseResult = await _userService.GetAllUsersAsync(userParameters);
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
        var baseResult = await _userService.DeleteUserAsync(email);
        if(!baseResult.Suссess)
            return ProccessError(baseResult);
        
        return Ok();
    }
    
    [HttpPost("block/{email}")]
    public async Task<IActionResult> BlockUser(string email)
    {
        var baseResult = await _userService.BlockUserAsync(email);
        if(!baseResult.Suссess)
            return ProccessError(baseResult);
        
        return Ok();
    }
    
    [HttpPost("unblock/{email}")]
    public async Task<IActionResult> UnblockUser(string email)
    {
        var baseResult = await _userService.UnblockUserAsync(email);
        if(!baseResult.Suссess)
            return ProccessError(baseResult);
        
        return Ok();
    }
}