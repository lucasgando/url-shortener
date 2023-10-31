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
    public class UrlShortenerController : Controller
    {
        private readonly UrlService _service;
        private readonly UserService _userService;
        public UrlShortenerController(UrlService urlService, UserService userService)
        {
            _userService = userService;
            _service = urlService;
        }
        [HttpGet("urls")]
        public IActionResult GetUrls()
        {
            return Ok(_service.GetAll());
        }
        [HttpGet("urls/{id}")]
        public IActionResult GetUrl(int id)
        {
            Url? url = _service.GetById(id);
            if (url != null)
                return Ok(url);
            return NotFound();
        }
        [HttpGet("{code}")]
        [AllowAnonymous]
        public IActionResult RedirectToUrl(string code)
        {
            Url? url = _service.GetByCode(code);
            if (url == null) return NotFound();
            _service.UpdateClicks(url.Id);
            return Redirect(url.FullUrl);
        }
        [HttpPost]
        public IActionResult Shorten([FromBody] UrlForCreationDto url)
        {
            return Ok(_service.Add(url));
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_service.Delete(id))
                return Ok();
            return NotFound();
        }
    }
}
