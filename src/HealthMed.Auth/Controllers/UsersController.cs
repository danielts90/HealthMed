using HealthMed.Auth.Entities;
using HealthMed.Auth.Interfaces.Services;
using HealthMed.Auth.ViewModels;
using HealthMed.Infra.Logs.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILoggerService<UsersController> _logger;

        public UsersController(IUserService userService, 
                               ILoggerService<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel login)
        {
            try
            {
                var token = await _userService.Login(login.UserName, login.Password);
                _logger.Log($"User {login.UserName} logged in successfully.");
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
                var token = await _userService.CreateUser(user);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
