using Microsoft.AspNetCore.Mvc;
using ProjectCarTest.Interfaces;
using ProjectCarTest.Models;
using System.Collections.Generic;

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

        [HttpGet("all")]
        public IActionResult GetAllUsers()
        {
            var users = _userRepo.GetAllUsers();
            return Ok(users);
        }
    }
}
