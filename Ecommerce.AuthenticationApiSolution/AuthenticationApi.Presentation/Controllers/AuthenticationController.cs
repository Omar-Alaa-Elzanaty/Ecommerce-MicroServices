using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using eCommerce.SharedLibrary.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IUser userInterface):ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Response>>Register([FromBody] AppUserDto appUser)
        {
            var result = await userInterface.Register(appUser);

            if (!result.Flag)
                return BadRequest(result);

            return Ok(result);
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<Response>>Login([FromBody] LoginDto appUser)
        {
            var result = await userInterface.Login(appUser);

            if (!result.Flag)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<GetUserDto>>GetUser(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid user ID.");

            var user = await userInterface.GetUser(id);

            return user.Id > 0 ? Ok(user) : NotFound();
        }
    }
}
