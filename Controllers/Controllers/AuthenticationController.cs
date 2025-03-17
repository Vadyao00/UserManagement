using Contracts.IServices;
using Controllers.Extensions;
using Controllers.Filters;
using Domain.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController(IServiceManager service) : ApiControllerBase
    {
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var baseResult = await service.AuthenticationService.RegisterUser(userForRegistration);
            if(!baseResult.Suссess)
                return ProccessError(baseResult);
            
            return StatusCode(201);
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await service.AuthenticationService.ValidateUser(user))
                return Unauthorized();

            var tokenDto = await service.AuthenticationService.CreateToken(populateExp: true);

            return Ok(tokenDto);
        }
    }
}