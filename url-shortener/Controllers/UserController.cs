using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using url_shortener.Data.Entities;
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
        [HttpGet]
        public IActionResult GetAll()
        {
            string userRole = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)!.Value;
            if (userRole != "Admin") return Forbid();
            return Ok(_service.GetAll());
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            string userRole = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)!.Value;
            if (userRole != "Admin") return Forbid();
            UserDto? user = _service.GetById(id);
            if (user is not null) return Ok(user);
            return NotFound("User not found");
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Create([FromBody] UserForCreationDto dto)
        {
            _service.Add(dto);
            return Ok();
        }
        [HttpPut]
        public IActionResult Update([FromBody] UserForUpdateDto dto)
        {
            string userRole = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)!.Value;
            string email = User.Claims.FirstOrDefault(claim => claim.Type.Contains("email"))!.Value;
            if (userRole != "Admin" || email != dto.Email) return Forbid();
            _service.Update(dto);
            return Ok();
        }
        [HttpDelete]
        public IActionResult Delete([FromBody] UserForDeletionDto dto)
        {
            string userRole = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)!.Value;
            string email = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)!.Value;
            if (userRole != "Admin" || email != dto.Email) return Forbid();
            if (!_service.Exists(dto.Email)) return NotFound("User not found");
            bool result = _service.Delete(dto);
            if (result) return Ok();
            return BadRequest();
        }
    }
}
