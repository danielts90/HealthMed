using HealthMed.Auth.Entities;
using HealthMed.Auth.Interfaces.Services;
using HealthMed.Auth.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel login)
        {
            try
            {
                var token = await userService.Login(login.UserName, login.Password);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            try
            {
                var token = await userService.CreateUser(user);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
