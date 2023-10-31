using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return Ok(_service.GetAll());
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            User? user = _service.GetById(id);
            if (user != null)
                return Ok(user);
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
            if (!_service.Exists(dto.Email)) return NotFound("User not found");
            _service.Update(dto);
            return Ok();
        }
        [HttpDelete]
        public IActionResult Delete([FromBody] UserForDeletionDto dto)
        {
            if (!_service.Exists(dto.Email)) return NotFound("User not found");
            bool result = _service.Delete(dto);
            if (result) return Ok();
            return BadRequest();
        }
    }
}
