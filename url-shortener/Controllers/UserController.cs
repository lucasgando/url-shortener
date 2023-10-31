using Microsoft.AspNetCore.Mvc;
using url_shortener.Data.Entities;
using url_shortener.Data.Models;
using url_shortener.Services;

namespace url_shortener.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _service;
        public UserController(UserService service)
        {
            _service = service;
        }
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            return Ok(_service.GetUsers());
        }
        [HttpGet("users/{id}")]
        public IActionResult GetUserById(int id)
        {
            User? user = _service.GetUser(id);
            if (user != null)
                return Ok(user);
            return NotFound("User not found");
        }
        [HttpPost("user")]
        public IActionResult CreateUser([FromBody] UserDto dto)
        {
            User userToCreate = new User()
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password
            };
            return Ok(_service.AddUser(userToCreate));
        }
    }
}
