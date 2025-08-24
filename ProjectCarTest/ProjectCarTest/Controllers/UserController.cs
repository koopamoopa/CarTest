using Microsoft.AspNetCore.Mvc;
using ProjectCarTest.Interfaces;
using ProjectCarTest.Models;

namespace ProjectCarTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User loginRequest)
        {
            var user = _userRepo.GetUserByUsernameAndPassword(loginRequest.username, loginRequest.password);

            if (user == null)
                return Unauthorized("Invalid username or password");


            return Ok(user);
        }
    }
}
