using Microsoft.AspNetCore.Mvc;
using ProjectCarTest.Dto;
using ProjectCarTest.Interfaces;
using ProjectCarTest.Models;

// Handles HTTP requests and responses for Users Logging In
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
        public IActionResult Login([FromBody] LoginRequestDto loginDto)
        {
            var user = _userRepo.Login(loginDto);

            if (user == null)
                return Unauthorized("Invalid username or password");

            // Manually map User entity to LoginResponseDto
            var response = new LoginResponseDto
            {
                Token = user.Token,
                Username = user.Username,
                CompanyName = user.CompanyName,
                Result = user.Result
            };

            return Ok(response);
        }
    }
}
