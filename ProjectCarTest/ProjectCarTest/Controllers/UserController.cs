using Microsoft.AspNetCore.Mvc;
using ProjectCarTest.Dto;
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
        public IActionResult Login([FromBody] LoginRequestDto loginDto)
        {
            var user = _userRepo.GetUserByUsernameAndPassword(loginDto.Username, loginDto.Password);

            if (user == null)
                return Unauthorized("Invalid username or password");

            // Manually map User entity to LoginResponseDto
            var response = new LoginResponseDto
            {
                Username = user.username,
                CompanyName = user.companyName
            };

            return Ok(response);
        }
    }
}
