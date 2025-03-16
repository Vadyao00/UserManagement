using Contracts.IServices;
using Domain.Dtos;
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
        var users = await _userService.GetAllUsersAsync(userParameters);
        var userDtos = new List<UserDto>();

        foreach (var user in users)
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
        var user = await _userService.GetUserByEmailAsync(email);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        await _userService.DeleteUserAsync(email);

        return Ok();
    }
    
    [HttpPost("block/{email}")]
    public async Task<IActionResult> BlockUser(string email)
    {
        await _userService.BlockUserAsync(email);
        return Ok();
    }
    
    [HttpPost("unblock/{email}")]
    public async Task<IActionResult> UnblockUser(string email)
    {
        await _userService.UnblockUserAsync(email);
        return Ok();
    }
}