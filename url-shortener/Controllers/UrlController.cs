using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using url_shortener.Data.Implementations;
using url_shortener.Data.Models.Dtos.Url;
using url_shortener.Services;

namespace url_shortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UrlController : BaseController
    {
        private readonly UrlService _service;
        private readonly CategoryService _categoryService;
        public UrlController(UrlService urlService, CategoryService categoryService)
        {
            _service = urlService;
            _categoryService = categoryService;
        }
        [HttpGet("admin/urls")]
        public IActionResult GetAll()
        {
            if (!Admin()) return Forbid();
            return Ok(_service.GetAll());
        }
        [HttpGet("admin/urls/{id}")]
        public IActionResult GetById(int id)
        {
            if (!Admin()) return Forbid();
            UrlDto? url = _service.GetById(id);
            if (url is not null) return Ok(url);
            return NotFound();
        }
        [HttpGet("urls")]
        public IActionResult GetUserUrls()
        {
            List<UrlDto> urls = _service.GetByUserId(UserId());
            return Ok(urls);
        }
        [HttpGet("urls/{categoryName}")]
        public IActionResult GetByCategory(string categoryName)
        {
            if (!Admin() && !_categoryService.UserIsOwner(UserId(), categoryName)) return Forbid();
            return Ok(_service.GetByCategory(categoryName));
        }
        [HttpPut]
        public IActionResult UpdateUrlCategory(UrlForUpdateDto dto)
        {
            if (!_service.UserIsOwner(UserId(), dto.Id)) return Forbid();
            _service.ChangeCategories(dto);
            return NoContent();
        }
        [HttpPost]
        public IActionResult Shorten([FromBody] UrlForCreationDto url)
        {
            if (url.Url.Length > 500) return BadRequest("Url too long");
            int urlId = _service.Add(url, UserId());
            return Created("", urlId);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            UrlDto? url = _service.GetById(id);
            if (url is null) return NotFound();
            if (UserId() != url.UserId) return Forbid();
            _service.Delete(id);
            return Ok();
        }
    }
}
