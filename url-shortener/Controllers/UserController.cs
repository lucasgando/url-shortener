using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using url_shortener.Data.Models.Dtos;
using url_shortener.Services;

namespace url_shortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserService _service;
        public UserController(UserService service)
        {
            _service = service;
        }
        [HttpGet("admin/users")]
        public IActionResult GetAll()
        {
            string userRole = User.Claims.First(claim => claim.Type.Contains("role")).Value;
            if (userRole != "Admin") return Forbid();
            return Ok(_service.GetAll());
        }
        [HttpGet("admin/users/{id}")]
        public IActionResult GetById(int id)
        {
            string userRole = User.Claims.First(claim => claim.Type.Contains("role")).Value;
            if (userRole != "Admin") return Forbid();
            UserDto? user = _service.GetById(id);
            if (user is not null) return Ok(user);
            return NotFound("User not found");
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Create([FromBody] UserForCreationDto dto)
        {
            if (_service.Exists(dto.Email)) return Conflict("Email taken");
            _service.Add(dto);
            UserDto newUser = _service.GetByEmail(dto.Email)!;
            return Created(newUser.Id.ToString(), newUser);
        }
        [HttpPut]
        public IActionResult Update([FromBody] UserForUpdateDto dto)
        {
            string userRole = User.Claims.First(claim => claim.Type.Contains("role")).Value;
            string email = User.Claims.First(claim => claim.Type.Contains("email")).Value;
            Console.WriteLine(userRole);
            if (userRole != "Admin" && email != dto.Email) return Forbid();
            _service.Update(dto);
            return NoContent();
        }
        [HttpDelete]
        public IActionResult Delete([FromBody] UserForDeletionDto dto)
        {
            string userRole = User.Claims.First(claim => claim.Type.Contains("role")).Value;
            string email = User.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;
            Console.WriteLine("Delete used by:", userRole);
            if (userRole != "Admin" && email != dto.Email) return Forbid();
            if (!_service.Exists(dto.Email)) return NotFound("User not found");
            _service.Delete(dto);
            return NoContent();
        }
    }
}
