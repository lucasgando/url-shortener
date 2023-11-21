using Microsoft.AspNetCore.Mvc;
using url_shortener.Data.Implementations;
using url_shortener.Data.Models.Dtos.Category;
using url_shortener.Services;

namespace url_shortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : BaseController
    {
        private readonly CategoryService _service;
        public CategoryController(CategoryService service) { _service = service; }
        [HttpGet("admin/categories")]
        public IActionResult GetAll()
        {
            if (!Admin()) return Forbid();
            return Ok(_service.GetAll());
        }
        [HttpGet("admin/categories/{id}")]
        public IActionResult GetById(int id)
        {
            if (!Admin()) return Forbid();
            return Ok(_service.GetById(id));
        }
        [HttpGet("categories")]
        public IActionResult GetByUser()
        {
            return Ok(_service.GetByUser(UserId()));
        }
        [HttpPost]
        public IActionResult Add([FromBody] CategoryForCreationDto dto)
        {
            if (!_service.NameAvailable(dto.Name)) return Conflict();
            int categoryId = _service.Add(dto, UserId());
            return Created("categories/", categoryId);
        }
        [HttpPut]
        public IActionResult Update([FromBody] CategoryForUpdateDto dto)
        {
            if (!_service.NameAvailable(dto.Name)) return Conflict();
            if (!_service.UserIsOwner(UserId(), dto.Id)) return Forbid();
            _service.Update(dto);
            return NoContent();
        }
        [HttpDelete]
        public IActionResult Delete([FromBody] int id)
        {
            if (!_service.UserIsOwner(UserId(), id)) return Forbid();
            _service.Delete(id);
            return NoContent();
        }
    }
}
