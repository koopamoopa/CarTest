using Microsoft.AspNetCore.Mvc;
using ProjectCarTest.Dto;
using ProjectCarTest.Interfaces;
using ProjectCarTest.Models;
using ProjectCarTest.Utilities;
using static ProjectCarTest.Utilities.Helper;

// Handles HTTP requests and responses for Users Logging In
namespace ProjectCarTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private int maxUsernameLength = 50;
        private int maxPasswordLength = 50;


        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto loginDto)
        {
            // Input Validation
            if (loginDto.Username.Length > maxUsernameLength || loginDto.Password.Length > maxPasswordLength) // not accept if input exceeds defined max size
                return Unauthorized("Invalid username or password - Input Length exceeded String Length!" );
            if (!StringValidator.ContainsOnlyLegalCharacters(loginDto.Username) || !StringValidator.ContainsOnlyLegalCharacters(loginDto.Password)) // not accept any illegal characters
                return Unauthorized("Invalid username or password - Input contains Illegal Characters!");

            var user = _userRepo.Login(loginDto);

            // Input Validation for if username and password has no matches in database
            if (user.Result == "Fail")
                return Unauthorized("Invalid username or password - Input was Not Found!");

            // User exists
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
